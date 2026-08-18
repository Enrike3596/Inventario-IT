using DTOs;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemisionesController : ControllerBase
    {
        private readonly IRemisionService _service;

        public RemisionesController(IRemisionService service)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var data = await _service.ObtenerDetalleAsync(id);
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
        public async Task<IActionResult> Crear([FromBody] RemisionCreateDTO dto)
        {
            try
            {
                var data = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = data.IdRemision }, ResponseHelper.Created(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] RemisionUpdateDTO dto)
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
                return Ok(ResponseHelper.Success(null, "Remisión eliminada exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarIngreso(int id)
        {
            try
            {
                var data = await _service.ConfirmarIngresoAsync(id);
                return Ok(ResponseHelper.Success(data, "Ingreso confirmado. Activos creados exitosamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.BadRequest(ex.Message));
            }
        }
    }
}