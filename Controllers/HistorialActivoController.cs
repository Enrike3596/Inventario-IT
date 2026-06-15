using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialActivoController : ControllerBase
    {
        private readonly IHistorialActivoService _service;

        public HistorialActivoController(IHistorialActivoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var data = await _service.ObtenerTodosAsync();
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpGet("activo/{idActivo}")]
        public async Task<IActionResult> ObtenerPorActivo(int idActivo)
        {
            try
            {
                var data = await _service.ObtenerPorActivoAsync(idActivo);
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
