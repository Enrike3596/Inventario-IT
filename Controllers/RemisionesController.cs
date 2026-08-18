using DTOs;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories;
using Services;
using Services.FileStorage;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemisionesController : ControllerBase
    {
        private const string ContenedorDocumentos = "remisiones";

        private readonly IRemisionService _service;
        private readonly IRemisionRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly FileStorageSettings _storageSettings;

        public RemisionesController(
            IRemisionService service,
            IRemisionRepository repo,
            IFileStorageService fileStorage,
            IOptions<FileStorageSettings> storageSettings)
        {
            _service = service;
            _repo = repo;
            _fileStorage = fileStorage;
            _storageSettings = storageSettings.Value;
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

        [Authorize]
        [HttpPost("documento")]
        public async Task<IActionResult> SubirDocumento(IFormFile file)
        {
            try
            {
                var (ruta, nombre) = await GuardarYValidarDocumentoAsync(file);
                return Ok(ResponseHelper.Success(new RemisionDocumentoDTO
                {
                    RutaDocumento = ruta,
                    NombreDocumento = nombre
                }, "Documento subido exitosamente."));
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

        [Authorize]
        [HttpDelete("documento")]
        public async Task<IActionResult> EliminarDocumentoTemporal([FromQuery] string path)
        {
            try
            {
                if (!EsRutaSegura(path))
                    return BadRequest(ResponseHelper.BadRequest("Ruta de documento inválida."));

                var (contenedor, nombreArchivo) = DescomponerRuta(path);
                await _fileStorage.DeleteAsync(contenedor, nombreArchivo);
                return Ok(ResponseHelper.Success(null, "Documento temporal eliminado."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpPost("{id}/documento")]
        public async Task<IActionResult> ReemplazarDocumento(int id, IFormFile file)
        {
            try
            {
                var remision = await _repo.ObtenerPorIdAsync(id);
                if (remision == null)
                    return NotFound(ResponseHelper.NotFound());

                var (ruta, nombre) = await GuardarYValidarDocumentoAsync(file);

                if (!string.IsNullOrWhiteSpace(remision.RutaDocumento)
                    && !string.Equals(remision.RutaDocumento, ruta, StringComparison.OrdinalIgnoreCase))
                {
                    var (contenedorViejo, nombreViejo) = DescomponerRuta(remision.RutaDocumento);
                    await _fileStorage.DeleteAsync(contenedorViejo, nombreViejo);
                }

                await _repo.ActualizarDocumentoAsync(id, ruta, nombre);
                return Ok(ResponseHelper.Success(new RemisionDocumentoDTO
                {
                    RutaDocumento = ruta,
                    NombreDocumento = nombre
                }, "Documento reemplazado exitosamente."));
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

        [Authorize]
        [HttpGet("{id}/documento")]
        public async Task<IActionResult> ObtenerDocumento(int id, [FromQuery] bool descarga = false)
        {
            try
            {
                var remision = await _repo.ObtenerPorIdAsync(id);
                if (remision == null)
                    return NotFound(ResponseHelper.NotFound());
                if (string.IsNullOrWhiteSpace(remision.RutaDocumento))
                    return NotFound(ResponseHelper.NotFound("La remisión no tiene documento."));

                var (contenedor, nombreArchivo) = DescomponerRuta(remision.RutaDocumento);
                var stream = await _fileStorage.GetAsync(contenedor, nombreArchivo);
                if (stream == null)
                    return NotFound(ResponseHelper.NotFound("No se encontró el archivo del documento."));

                if (descarga)
                {
                    var nombre = string.IsNullOrWhiteSpace(remision.NombreDocumento)
                        ? $"remision-{id}.pdf"
                        : remision.NombreDocumento;
                    return File(stream, "application/pdf", nombre);
                }

                Response.Headers.Append("Content-Disposition", $"inline; filename=\"{remision.NombreDocumento ?? $"remision-{id}.pdf"}\"");
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("{id}/documento")]
        public async Task<IActionResult> EliminarDocumento(int id)
        {
            try
            {
                var remision = await _repo.ObtenerPorIdAsync(id);
                if (remision == null)
                    return NotFound(ResponseHelper.NotFound());

                if (!string.IsNullOrWhiteSpace(remision.RutaDocumento))
                {
                    var (contenedor, nombreArchivo) = DescomponerRuta(remision.RutaDocumento);
                    await _fileStorage.DeleteAsync(contenedor, nombreArchivo);
                }

                await _repo.ActualizarDocumentoAsync(id, null, null);
                return Ok(ResponseHelper.Success(null, "Documento eliminado exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseHelper.Error(ex.Message));
            }
        }

        private async Task<(string Ruta, string Nombre)> GuardarYValidarDocumentoAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Debe adjuntar un archivo PDF.");

            if (file.Length > _storageSettings.DocumentoRemisionMaxBytes)
                throw new InvalidOperationException($"El archivo excede el tamaño máximo permitido ({(double)_storageSettings.DocumentoRemisionMaxBytes / (1024 * 1024):0} MB).");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_storageSettings.DocumentoRemisionExtensiones.Contains(extension, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("Solo se permiten archivos PDF.");

            await using var stream = file.OpenReadStream();
            var buffer = new byte[5];
            var leidos = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length));
            if (leidos < 5 || buffer[0] != '%' || buffer[1] != 'P' || buffer[2] != 'D' || buffer[3] != 'F' || buffer[4] != '-')
                throw new InvalidOperationException("El archivo no es un PDF válido.");

            stream.Position = 0;
            var nombreArchivo = Path.GetFileName(file.FileName);
            var ruta = await _fileStorage.SaveAsync(ContenedorDocumentos, nombreArchivo, stream);
            return (ruta, nombreArchivo);
        }

        private static bool EsRutaSegura(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return false;
            var limpia = ruta.Replace('\\', '/');
            return limpia.StartsWith(ContenedorDocumentos + "/", StringComparison.OrdinalIgnoreCase)
                && !limpia.Contains("..");
        }

        private static (string Contenedor, string NombreArchivo) DescomponerRuta(string ruta)
        {
            var limpia = ruta.Replace('\\', '/');
            var idx = limpia.IndexOf('/');
            if (idx <= 0 || idx == limpia.Length - 1)
                return ("", limpia);
            return (limpia[..idx], limpia[(idx + 1)..]);
        }
    }
}