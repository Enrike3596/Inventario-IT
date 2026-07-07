using DTOs;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesItemOCController : ControllerBase
    {
        private readonly IDetalleItemOCService _service;

        public DetallesItemOCController(IDetalleItemOCService service)
        {
            _service = service;
        }

        [HttpGet("item/{idItemOC}")]
        public async Task<IActionResult> ObtenerPorItem(int idItemOC)
        {
            try
            {
                var data = await _service.ObtenerPorItemAsync(idItemOC);
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
        public async Task<IActionResult> Crear([FromBody] DetalleItemOCCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = data.IdDetalleItemOC }, ResponseHelper.Created(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CrearBatch([FromBody] DetalleItemOCBatchCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearBatchAsync(dto.IdItemOC, dto.Seriales);
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetalleItemOCUpdateDTO dto)
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
                return Ok(ResponseHelper.Success(null, "Detalle eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }
    }
}
