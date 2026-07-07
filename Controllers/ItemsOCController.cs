using DTOs;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsOCController : ControllerBase
    {
        private readonly IItemOCService _service;

        public ItemsOCController(IItemOCService service)
        {
            _service = service;
        }

        [HttpGet("orden/{idOrden}")]
        public async Task<IActionResult> ObtenerPorOrden(int idOrden)
        {
            try
            {
                var data = await _service.ObtenerPorOrdenAsync(idOrden);
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

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ItemOCCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = data.IdItemOC }, ResponseHelper.Created(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ItemOCUpdateDTO dto)
        {
            try
            {
                var data = await _service.ActualizarAsync(id, dto);
                if (data == null)
                    return NotFound(ResponseHelper.NotFound());
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var result = await _service.EliminarAsync(id);
                if (!result)
                    return NotFound(ResponseHelper.NotFound());
                return Ok(ResponseHelper.Success(null, "Item eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
