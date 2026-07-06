using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTOs;
using Enums;
using Helpers;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;

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
        Task<string> SolicitarRestablecimientoAsync(SolicitarRestablecimientoDTO dto);
        Task RestablecerContrasenaAsync(RestablecerContrasenaDTO dto);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
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
            var usuario = new Usuarios
            {
                IdRol = dto.IdRol,
                IdSede = dto.IdSede,
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Cargo = dto.Cargo,
                Contraseña = PasswordHelper.Hash(dto.Contraseña)
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
            var usuario = await _repo.ObtenerPorCorreoAsync(dto.Correo);
            if (usuario == null)
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            if (usuario.EstadoUsuario != EstadoUsuario.Activo)
                throw new UnauthorizedAccessException("El usuario se encuentra inactivo.");

            if (!PasswordHelper.Verify(dto.Contraseña, usuario.Contraseña))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            var expira = DateTime.UtcNow.AddHours(8);

            return new TokenResponseDTO
            {
                Token = GenerateToken(usuario, expira),
                Expira = expira,
                Usuario = MapToDTO(usuario)
            };
        }

        public async Task<string> SolicitarRestablecimientoAsync(SolicitarRestablecimientoDTO dto)
        {
            var usuario = await _repo.ObtenerPorCorreoAsync(dto.Correo);
            if (usuario == null)
                throw new KeyNotFoundException("No se encontró un usuario con ese correo.");

            if (usuario.EstadoUsuario != EstadoUsuario.Activo)
                throw new UnauthorizedAccessException("El usuario se encuentra inactivo.");

            var expira = DateTime.UtcNow.AddHours(1);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("propósito", "restablecimiento_contraseña")
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

        public async Task RestablecerContrasenaAsync(RestablecerContrasenaDTO dto)
        {
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
                    IssuerSigningKey = key
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
                IdSede = u.IdSede,
                NombreSede = u.Sede?.Nombre,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Telefono = u.Telefono,
                Cargo = u.Cargo,
                EstadoUsuario = u.EstadoUsuario,
                FechaCreacion = u.FechaCreacion
            };
        }
    }
}
