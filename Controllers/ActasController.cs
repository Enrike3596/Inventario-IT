using DTOs;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ActaFirma;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActasController : ControllerBase
    {
        private readonly IActaFirmaService _service;

        public ActasController(IActaFirmaService service)
        {
            _service = service;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generar([FromBody] DestinoRequest request)
        {
            try
            {
                var data = await _service.GenerarActaAsync(request.IdDestino, request.TipoDestino);
                return Ok(ResponseHelper.Success(data, "Acta generada exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> Enviar([FromBody] DestinoRequest request)
        {
            try
            {
                var data = await _service.EnviarParaFirmaAsync(request.IdDestino, request.TipoDestino);
                return Ok(ResponseHelper.Success(data, "Acta enviada para firma exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpGet("destino")]
        public async Task<IActionResult> ObtenerPorDestino([FromQuery] string tipo, [FromQuery] int id)
        {
            try
            {
                var data = await _service.ObtenerPorDestinoAsync(id, tipo);
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpDelete("destino")]
        public async Task<IActionResult> Eliminar([FromQuery] string tipo, [FromQuery] int id)
        {
            try
            {
                await _service.EliminarActaAsync(id, tipo);
                return Ok(ResponseHelper.Success(null, "Acta eliminada exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("firmar/{token}")]
        public async Task<IActionResult> ObtenerParaFirma(string token)
        {
            try
            {
                var data = await _service.ObtenerPorTokenAsync(token);
                if (data == null)
                    return NotFound(ResponseHelper.NotFound("Enlace inválido o expirado."));
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("firmar/{token}")]
        public async Task<IActionResult> Firmar(string token, [FromBody] FirmaRequestDTO dto)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
                var data = await _service.FirmarAsync(token, dto, ip);
                return Ok(ResponseHelper.Success(data, "Acta firmada exitosamente."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
