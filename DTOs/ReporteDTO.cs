namespace DTOs
{
    public class ReporteInventarioRequest
    {
        public List<string> Columnas { get; set; } = new();

        public FiltrosInventario? Filtros { get; set; }

        public string? AgrupadoPor { get; set; }

        public string? OrdenadoPor { get; set; }

        public bool OrdenDescendente { get; set; } = false;

        public int? PaginaPreview { get; set; } = 1;

        public int? TamPaginaPreview { get; set; } = 50;
    }

    public class FiltrosInventario
    {
        public List<string>? Categoria { get; set; }

        public List<string>? Estado { get; set; }

        public List<string>? Sede { get; set; }

        public List<string>? Area { get; set; }

        public List<int>? ResponsableId { get; set; }

        public DateTime? FechaAdquisicionDesde { get; set; }

        public DateTime? FechaAdquisicionHasta { get; set; }

        public string? Proveedor { get; set; }

        public string? NumeroRemision { get; set; }
    }

    public class ReportePreviewResponse
    {
        public List<string> Columnas { get; set; } = new();

        public List<Dictionary<string, object?>> Filas { get; set; } = new();

        public int TotalRegistros { get; set; }

        public int PaginaActual { get; set; }

        public int TotalPaginas { get; set; }
    }
}
