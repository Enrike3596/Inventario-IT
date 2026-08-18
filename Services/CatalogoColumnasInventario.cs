using Enums;
using Models;

namespace Services
{
    public static class CatalogoColumnasInventario
    {
        public static readonly Dictionary<string, ColumnaDefinicion> Columnas = new()
        {
            ["codigoActivo"]      = new("Código activo",    a => a.CodigoActivo),
            ["serial"]            = new("Serial",           a => a.Serial),
            ["marca"]             = new("Marca",            a => a.Marca),
            ["modelo"]            = new("Modelo",           a => a.Modelo),
            ["categoria"]         = new("Categoría",        a => a.Categoria.Nombre),
            ["estado"]            = new("Estado",           a => EtiquetaEstado(EstadoEfectivoActivo(a))),
            ["fechaAdquisicion"]  = new("Fecha adquisición", a => a.FechaAdquisicion.ToString("yyyy-MM-dd")),
            ["fechaBaja"]         = new("Fecha baja",       a => a.FechaBaja?.ToString("yyyy-MM-dd") ?? "-"),
            ["numeroRemision"]    = new("N° remisión",     a => a.Remision.NumeroRemision),
            ["proveedor"]         = new("Proveedor",        a => a.Remision.Proveedor),
            ["fechaCompra"]       = new("Fecha compra",     a => a.Remision.FechaCompra.ToString("yyyy-MM-dd")),
            ["responsable"]       = new("Responsable",      a => ResponsableActivo(a)),
            ["area"]              = new("Área",             a => AreaResponsable(a)),
            ["observaciones"]     = new("Observaciones",    a => a.Observaciones ?? "-")
        };

        public record ColumnaDefinicion(string Etiqueta, Func<Activos, object?> Selector);

        public static bool EsColumnaValida(string? clave)
            => !string.IsNullOrWhiteSpace(clave) && Columnas.ContainsKey(clave);

        private static string ResponsableActivo(Activos a)
            => a.AsignacionesUsuario
                .FirstOrDefault(au => au.EstadoAsignacion == EstadoAsignacion.Activa)
                ?.Usuario?.Nombre ?? "Sin asignar";

        private static string AreaResponsable(Activos a)
            => a.AsignacionesUsuario
                .FirstOrDefault(au => au.EstadoAsignacion == EstadoAsignacion.Activa)
                ?.Usuario?.Area?.NombreArea ?? "-";

        public static EstadoActivo EstadoEfectivoActivo(Activos a)
            => a.EstadoActivo == EstadoActivo.Disponible &&
               a.AsignacionesUsuario.Any(au => au.EstadoAsignacion == EstadoAsignacion.Activa)
                ? EstadoActivo.Asignado
                : a.EstadoActivo;

        public static string EtiquetaEstado(EstadoActivo estado) => estado switch
        {
            EstadoActivo.Disponible => "Disponible",
            EstadoActivo.Asignado => "Asignado",
            EstadoActivo.EnReparacion => "En reparación",
            EstadoActivo.DadoDeBaja => "Dado de baja",
            EstadoActivo.Venta => "Venta",
            _ => estado.ToString()
        };
    }
}
