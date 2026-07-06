using System.Security.Claims;
using DTOs;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
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
    }
}
