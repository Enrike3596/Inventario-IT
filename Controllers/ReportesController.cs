using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/reportes")]
    public class ReportesController : ControllerBase
    {
        private readonly ReporteInventarioService _reporteService;
        private readonly IReporteExportador _exportador;

        public ReportesController(ReporteInventarioService reporteService, IReporteExportador exportador)
        {
            _reporteService = reporteService;
            _exportador = exportador;
        }

        [HttpPost("preview")]
        public async Task<ActionResult<ReportePreviewResponse>> Preview([FromBody] ReporteInventarioRequest request)
        {
            var query = _reporteService.AplicarFiltros(request.Filtros);

            var total = await query.CountAsync();

            var pagina = request.PaginaPreview ?? 1;
            var tamPagina = request.TamPaginaPreview ?? 50;

            var activos = await query
                .Skip((pagina - 1) * tamPagina)
                .Take(tamPagina)
                .ToListAsync();

            var filas = _reporteService.ProyectarColumnas(activos, request.Columnas);
            filas = _reporteService.OrdenarFilas(filas, request.OrdenadoPor, request.AgrupadoPor, request.OrdenDescendente);

            return Ok(new ReportePreviewResponse
            {
                Columnas = request.Columnas
                    .Where(c => CatalogoColumnasInventario.Columnas.ContainsKey(c))
                    .Select(c => CatalogoColumnasInventario.Columnas[c].Etiqueta)
                    .ToList(),
                Filas = filas,
                TotalRegistros = total,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamPagina)
            });
        }

        [HttpPost("exportar")]
        public async Task<IActionResult> Exportar(
            [FromBody] ReporteInventarioRequest request,
            [FromQuery] string formato = "pdf")
        {
            var query = _reporteService.AplicarFiltros(request.Filtros);
            var activos = await query.ToListAsync();
            var filas = _reporteService.ProyectarColumnas(activos, request.Columnas);
            filas = _reporteService.OrdenarFilas(filas, request.OrdenadoPor, request.AgrupadoPor, request.OrdenDescendente);
            var columnas = request.Columnas
                .Where(c => CatalogoColumnasInventario.Columnas.ContainsKey(c))
                .Select(c => CatalogoColumnasInventario.Columnas[c].Etiqueta)
                .ToList();

            byte[] archivo;
            string contentType;
            string extension;

            switch (formato.ToLowerInvariant())
            {
                case "excel":
                    archivo = _exportador.GenerarExcel(columnas, filas);
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    extension = "xlsx";
                    break;
                case "pdf":
                    archivo = _exportador.GenerarPdf(columnas, filas);
                    contentType = "application/pdf";
                    extension = "pdf";
                    break;
                default:
                    return BadRequest($"Formato no soportado: '{formato}'. Use 'pdf' o 'excel'.");
            }

            var nombreArchivo = $"informe-inventario-{DateTime.Now:yyyyMMdd-HHmm}.{extension}";
            return File(archivo, contentType, nombreArchivo);
        }
    }
}
