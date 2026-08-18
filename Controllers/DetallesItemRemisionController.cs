using DTOs;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesItemRemisionController : ControllerBase
    {
        private readonly IDetalleItemRemisionService _service;

        public DetallesItemRemisionController(IDetalleItemRemisionService service)
        {
            _service = service;
        }

        [HttpGet("item/{idItemRemision}")]
        public async Task<IActionResult> ObtenerPorItem(int idItemRemision)
        {
            try
            {
                var data = await _service.ObtenerPorItemAsync(idItemRemision);
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
        public async Task<IActionResult> Crear([FromBody] DetalleItemRemisionCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = data.IdDetalleItemRemision }, ResponseHelper.Created(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CrearBatch([FromBody] DetalleItemRemisionBatchCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearBatchAsync(dto.IdItemRemision, dto.Seriales);
                return Ok(ResponseHelper.Success(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetalleItemRemisionUpdateDTO dto)
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