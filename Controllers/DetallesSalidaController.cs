using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesSalidaController : ControllerBase
    {
        private readonly IDetalleSalidaService _service;

        public DetallesSalidaController(IDetalleSalidaService service)
        {
            _service = service;
        }

        [HttpGet("salida/{idSalida}")]
        public async Task<IActionResult> ObtenerPorSalida(int idSalida)
        {
            try
            {
                var data = await _service.ObtenerPorSalidaAsync(idSalida);
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var data = await _service.ObtenerPorIdAsync(id);
                if (data == null)
                    return NotFound(ResponseHelper.NotFound());
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
