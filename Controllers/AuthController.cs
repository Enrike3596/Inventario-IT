using System.Security.Claims;
using DTOs;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IWebHostEnvironment _env;

        public AuthController(IUsuarioService usuarioService, IWebHostEnvironment env)
        {
            _usuarioService = usuarioService;
            _env = env;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _usuarioService.LoginAsync(dto);
            return Ok(ResponseHelper.Success(result));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var id))
                return Unauthorized(ResponseHelper.Error("Token inválido."));

            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound(ResponseHelper.NotFound("Usuario no encontrado."));

            return Ok(ResponseHelper.Success(usuario));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] SolicitarRestablecimientoDTO dto)
        {
            // Respuesta idéntica exista o no el correo: evita enumeración de cuentas.
            // El token solo se expone en Development para pruebas; en producción viaja por email.
            const string mensaje = "Si el correo existe, recibirás las instrucciones.";
            var token = await _usuarioService.SolicitarRestablecimientoAsync(dto);
            if (_env.IsDevelopment() && token != null)
                return Ok(ResponseHelper.Success(new { token }, mensaje));
            return Ok(ResponseHelper.Success(null, mensaje));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> RestablecerContrasena([FromBody] RestablecerContrasenaDTO dto)
        {
            await _usuarioService.RestablecerContrasenaAsync(dto);
            return Ok(ResponseHelper.Success(null, "Contraseña restablecida correctamente."));
        }
    }
}
