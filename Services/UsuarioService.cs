using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTOs;
using Enums;
using Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Services.Emails;

namespace Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioResponseDTO>> ObtenerTodosAsync();
        Task<UsuarioResponseDTO?> ObtenerPorIdAsync(int id);
        Task<UsuarioResponseDTO?> ObtenerPorCorreoAsync(string correo);
        Task<UsuarioResponseDTO> CrearAsync(UsuarioCreateDTO dto);
        Task<UsuarioResponseDTO?> ActualizarAsync(int id, UsuarioUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<TokenResponseDTO> LoginAsync(LoginDTO dto);
        Task<string?> SolicitarRestablecimientoAsync(SolicitarRestablecimientoDTO dto);
        Task RestablecerContrasenaAsync(RestablecerContrasenaDTO dto);
    }

    public class UsuarioService : IUsuarioService
    {
        // Tokens de restablecimiento ya consumidos (un solo uso). Caché en memoria:
        // válido para instancia única; con múltiples réplicas se requiere store distribuido.
        private static readonly ConcurrentDictionary<string, byte> _tokensRestablecimientoUsados = new();

        private readonly IUsuarioRepository _repo;
        private readonly IRolRepository _rolRepo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _cache;

        private const int MaxIntentosFallidos = 5;
        private static readonly TimeSpan BloqueoPorIntentos = TimeSpan.FromMinutes(15);

        private static string ClaveBloqueo(string correo) =>
            $"login-bloqueo:{correo.Trim().ToLowerInvariant()}";

        private static string ClaveIntentos(string correo) =>
            $"login-intentos:{correo.Trim().ToLowerInvariant()}";

        public UsuarioService(
            IUsuarioRepository repo,
            IRolRepository rolRepo,
            IConfiguration configuration,
            ILogger<UsuarioService> logger,
            IEmailSender emailSender,
            IMemoryCache cache)
        {
            _repo = repo;
            _rolRepo = rolRepo;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
            _cache = cache;
        }

        public async Task<List<UsuarioResponseDTO>> ObtenerTodosAsync()
        {
            var usuarios = await _repo.ObtenerTodosAsync();
            return usuarios.Select(MapToDTO).ToList();
        }

        public async Task<UsuarioResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var usuario = await _repo.ObtenerPorIdAsync(id);
            return usuario == null ? null : MapToDTO(usuario);
        }

        public async Task<UsuarioResponseDTO?> ObtenerPorCorreoAsync(string correo)
        {
            var usuario = await _repo.ObtenerPorCorreoAsync(correo);
            return usuario == null ? null : MapToDTO(usuario);
        }

        public async Task<UsuarioResponseDTO> CrearAsync(UsuarioCreateDTO dto)
        {
            var rol = await _rolRepo.ObtenerPorIdAsync(dto.IdRol);
            var esUsuarioFinal = rol?.Tipo == "usuario";

            var usuario = new Usuarios
            {
                IdRol = dto.IdRol,
                IdArea = dto.IdArea,
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Cargo = dto.Cargo,
                Contraseña = esUsuarioFinal && string.IsNullOrEmpty(dto.Contraseña)
                    ? PasswordHelper.Hash("SinAcceso123*")
                    : PasswordHelper.Hash(dto.Contraseña!)
            };

            var creado = await _repo.CrearAsync(usuario);
            return MapToDTO(creado);
        }

        public async Task<UsuarioResponseDTO?> ActualizarAsync(int id, UsuarioUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        public async Task<TokenResponseDTO> LoginAsync(LoginDTO dto)
        {
            // Mensaje único en todos los fallos para no revelar si el correo existe,
            // si está inactivo o si la contraseña falló (anti-enumeración).
            const string mensajeGenerico = "Credenciales inválidas.";
            var usuario = await _repo.ObtenerPorCorreoAsync(dto.Email);
            if (usuario == null)
            {
                _logger.LogWarning("Login fallido: correo no registrado ({Correo}).", dto.Email);
                throw new UnauthorizedAccessException(mensajeGenerico);
            }

            if (usuario.EstadoUsuario != EstadoUsuario.Activo)
            {
                _logger.LogWarning("Login fallido: usuario inactivo ({Correo}).", dto.Email);
                throw new UnauthorizedAccessException(mensajeGenerico);
            }

            if (usuario.Rol?.Tipo == "usuario")
            {
                _logger.LogWarning("Login fallido: rol sin acceso ({Correo}).", dto.Email);
                throw new UnauthorizedAccessException(mensajeGenerico);
            }

            // Bloqueo temporal tras intentos fallidos (anti fuerza bruta por cuenta).
            if (_cache.TryGetValue(ClaveBloqueo(dto.Email), out var bloqueoObj)
                && bloqueoObj is DateTimeOffset bloqueadoHasta
                && bloqueadoHasta > DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Login bloqueado por intentos fallidos ({Correo}).", dto.Email);
                throw new UnauthorizedAccessException(mensajeGenerico);
            }

            if (!PasswordHelper.Verify(dto.Password, usuario.Contraseña))
            {
                _logger.LogWarning("Login fallido: contraseña incorrecta ({Correo}).", dto.Email);
                RegistrarIntentoFallido(dto.Email);
                throw new UnauthorizedAccessException(mensajeGenerico);
            }

            _cache.Remove(ClaveIntentos(dto.Email));

            var expira = DateTime.UtcNow.AddHours(8);

            return new TokenResponseDTO
            {
                Token = GenerateToken(usuario, expira),
                Expira = expira,
                Usuario = MapToDTO(usuario)
            };
        }

        private void RegistrarIntentoFallido(string correo)
        {
            var claveIntentos = ClaveIntentos(correo);
            var intentos = _cache.Get<int?>(claveIntentos) ?? 0;
            intentos++;

            if (intentos >= MaxIntentosFallidos)
            {
                _cache.Set(ClaveBloqueo(correo), DateTimeOffset.UtcNow.Add(BloqueoPorIntentos), BloqueoPorIntentos);
                _cache.Remove(claveIntentos);
                _logger.LogWarning("Cuenta bloqueada 15 min por {Intentos} intentos fallidos ({Correo}).", intentos, correo);
            }
            else
            {
                _cache.Set(claveIntentos, intentos, BloqueoPorIntentos);
            }
        }

        public async Task<string?> SolicitarRestablecimientoAsync(SolicitarRestablecimientoDTO dto)
        {
            // Sin excepciones para correo inexistente o inactivo: el controller responde
            // siempre igual para no permitir enumeración de cuentas.
            var usuario = await _repo.ObtenerPorCorreoAsync(dto.Email);
            if (usuario == null || usuario.EstadoUsuario != EstadoUsuario.Activo)
            {
                _logger.LogWarning("Solicitud de restablecimiento para correo no válido o inactivo.");
                return null;
            }

            var expira = DateTime.UtcNow.AddHours(1);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("propósito", "restablecimiento_contraseña"),
                new Claim("jti", Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: credentials
            );

            var tokenTexto = new JwtSecurityTokenHandler().WriteToken(token);
            await EnviarCorreoRestablecimientoAsync(usuario.Correo, tokenTexto);
            return tokenTexto;
        }

        private async Task EnviarCorreoRestablecimientoAsync(string correo, string token)
        {
            try
            {
                var baseUrl = _configuration["PasswordReset:FrontendBaseUrl"]
                    ?? _configuration["FirmaElectronica:FrontendBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    _logger.LogWarning("No se envió correo de restablecimiento: falta PasswordReset:FrontendBaseUrl.");
                    return;
                }

                var enlace = $"{baseUrl.TrimEnd('/')}/reset-password?token={Uri.EscapeDataString(token)}";
                await _emailSender.SendAsync(
                    correo,
                    "Restablecimiento de contraseña — Inventario TI",
                    $"<p>Recibimos una solicitud para restablecer tu contraseña.</p>" +
                    $"<p><a href=\"{enlace}\">Haz clic aquí para definir una nueva contraseña</a>. " +
                    $"El enlace vence en 1 hora y solo puede usarse una vez.</p>" +
                    $"<p>Si no solicitaste este cambio, ignora este mensaje.</p>");
            }
            catch (Exception ex)
            {
                // No se propaga: la respuesta al cliente debe ser idéntica haya o no correo.
                _logger.LogError(ex, "Error al enviar correo de restablecimiento.");
            }
        }

        public async Task RestablecerContrasenaAsync(RestablecerContrasenaDTO dto)
        {
            // Un solo uso: rechaza tokens ya consumidos antes de validarlos.
            if (!_tokensRestablecimientoUsados.TryAdd(dto.Token, 0))
                throw new ArgumentException("El token ya fue utilizado.");

            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            try
            {
                var principal = handler.ValidateToken(dto.Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.FromMinutes(2)
                }, out _);

                var prop = principal.FindFirst("propósito")?.Value;
                if (prop != "restablecimiento_contraseña")
                    throw new ArgumentException("Token inválido para restablecimiento de contraseña.");

                var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var id))
                    throw new ArgumentException("Token inválido.");

                var hash = PasswordHelper.Hash(dto.NuevaContrasena);
                var actualizado = await _repo.ActualizarContrasenaAsync(id, hash);
                if (!actualizado)
                    throw new KeyNotFoundException("Usuario no encontrado.");
            }
            catch (SecurityTokenException)
            {
                throw new ArgumentException("El token ha expirado o es inválido.");
            }
        }

        private string GenerateToken(Usuarios usuario, DateTime expira)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                // El claim de rol lleva el Tipo (super_admin, coordinador, ...) para que
                // [Authorize(Roles=...)] se evalúe en servidor; se conserva el Nombre
                // por compatibilidad con sesiones ya emitidas.
                new Claim(ClaimTypes.Role, usuario.Rol?.Tipo ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? ""),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static UsuarioResponseDTO MapToDTO(Usuarios u)
        {
            return new UsuarioResponseDTO
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                NombreRol = u.Rol?.Nombre,
                IdArea = u.IdArea,
                NombreArea = u.Area?.NombreArea,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Cargo = u.Cargo,
                EstadoUsuario = u.EstadoUsuario,
                FechaCreacion = u.FechaCreacion,
                FechaModificacion = u.FechaModificacion,
                CreadoPor = u.CreadoPor,
                ModificadoPor = u.ModificadoPor
            };
        }
    }
}
