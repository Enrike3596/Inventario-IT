using DTOs;
using Data;
using Enums;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class ReporteInventarioService
    {
        private readonly AppDbContext _db;

        public ReporteInventarioService(AppDbContext db) => _db = db;

        public IQueryable<Activos> AplicarFiltros(FiltrosInventario? filtros)
        {
            var query = _db.Activos
                .Include(a => a.Categoria)
                .Include(a => a.Remision)
                .Include(a => a.AsignacionesUsuario)
                    .ThenInclude(au => au.Usuario)
                        .ThenInclude(u => u.Sede)
                .Include(a => a.AsignacionesUsuario)
                    .ThenInclude(au => au.Usuario)
                        .ThenInclude(u => u.Area)
                .AsQueryable();

            if (filtros is null) return query;

            if (filtros.Categoria?.Any() == true)
                query = query.Where(a => filtros.Categoria.Contains(a.Categoria.Nombre));

            if (filtros.Estado?.Any() == true)
            {
                var estados = filtros.Estado
                    .Select(s => Enum.TryParse<EstadoActivo>(s, true, out var e) ? e : (EstadoActivo?)null)
                    .Where(e => e.HasValue)
                    .Select(e => e!.Value)
                    .ToList();

                if (estados.Count > 0)
                {
                    var seleccionaAsignado = estados.Contains(EstadoActivo.Asignado);
                    var seleccionaDisponible = estados.Contains(EstadoActivo.Disponible);
                    query = query.Where(a =>
                        (a.EstadoActivo == EstadoActivo.Disponible &&
                         a.AsignacionesUsuario.Any(au => au.EstadoAsignacion == EstadoAsignacion.Activa) &&
                         seleccionaAsignado)
                        ||
                        (a.EstadoActivo == EstadoActivo.Disponible &&
                         !a.AsignacionesUsuario.Any(au => au.EstadoAsignacion == EstadoAsignacion.Activa) &&
                         seleccionaDisponible)
                        ||
                        (a.EstadoActivo != EstadoActivo.Disponible && estados.Contains(a.EstadoActivo)));
                }
            }

            if (filtros.Sede?.Any() == true)
                query = query.Where(a => a.AsignacionesUsuario.Any(au =>
                    au.EstadoAsignacion == EstadoAsignacion.Activa &&
                    au.Usuario.Sede != null &&
                    filtros.Sede.Contains(au.Usuario.Sede.Nombre)));

            if (filtros.Area?.Any() == true)
                query = query.Where(a => a.AsignacionesUsuario.Any(au =>
                    au.EstadoAsignacion == EstadoAsignacion.Activa &&
                    au.Usuario.Area != null &&
                    filtros.Area.Contains(au.Usuario.Area.NombreArea)));

            if (filtros.ResponsableId?.Any() == true)
                query = query.Where(a => a.AsignacionesUsuario.Any(au =>
                    au.EstadoAsignacion == EstadoAsignacion.Activa &&
                    filtros.ResponsableId.Contains(au.IdUsuarioDestino)));

            if (filtros.FechaAdquisicionDesde.HasValue)
                query = query.Where(a => a.FechaAdquisicion >= filtros.FechaAdquisicionDesde.Value);

            if (filtros.FechaAdquisicionHasta.HasValue)
                query = query.Where(a => a.FechaAdquisicion <= filtros.FechaAdquisicionHasta.Value);

            if (!string.IsNullOrWhiteSpace(filtros.Proveedor))
                query = query.Where(a => a.Remision.Proveedor.Contains(filtros.Proveedor));

            if (!string.IsNullOrWhiteSpace(filtros.NumeroRemision))
                query = query.Where(a => a.Remision.NumeroRemision.Contains(filtros.NumeroRemision));

            return query;
        }

        public List<Dictionary<string, object?>> ProyectarColumnas(
            IEnumerable<Activos> activos, List<string> columnasElegidas)
        {
            var columnasValidas = columnasElegidas
                .Where(c => CatalogoColumnasInventario.Columnas.ContainsKey(c))
                .ToList();

            var filas = new List<Dictionary<string, object?>>();

            foreach (var activo in activos)
            {
                var fila = new Dictionary<string, object?>();
                foreach (var col in columnasValidas)
                {
                    var def = CatalogoColumnasInventario.Columnas[col];
                    fila[def.Etiqueta] = def.Selector(activo);
                }
                filas.Add(fila);
            }

            return filas;
        }

        public List<Dictionary<string, object?>> OrdenarFilas(
            List<Dictionary<string, object?>> filas,
            string? ordenadoPor,
            string? agrupadoPor,
            bool ordenDescendente)
        {
            if (filas.Count == 0) return filas;

            var claveOrden = CatalogoColumnasInventario.EsColumnaValida(ordenadoPor) ? ordenadoPor : null;
            var claveGrupo = CatalogoColumnasInventario.EsColumnaValida(agrupadoPor) ? agrupadoPor : null;

            if (claveGrupo != null && claveGrupo != claveOrden)
            {
                var etiquetaGrupo = CatalogoColumnasInventario.Columnas[claveGrupo].Etiqueta;
                var etiquetaOrden = claveOrden != null
                    ? CatalogoColumnasInventario.Columnas[claveOrden].Etiqueta
                    : null;

                return filas
                    .GroupBy(f => f.GetValueOrDefault(etiquetaGrupo)?.ToString() ?? string.Empty)
                    .SelectMany(g =>
                    {
                        IEnumerable<Dictionary<string, object?>> grupoOrdenado = etiquetaOrden == null
                            ? g
                            : ordenDescendente
                                ? g.OrderByDescending(f => f.GetValueOrDefault(etiquetaOrden)?.ToString() ?? string.Empty)
                                : g.OrderBy(f => f.GetValueOrDefault(etiquetaOrden)?.ToString() ?? string.Empty);
                        return grupoOrdenado;
                    })
                    .ToList();
            }

            if (claveOrden != null)
            {
                var etiqueta = CatalogoColumnasInventario.Columnas[claveOrden].Etiqueta;
                return ordenDescendente
                    ? filas.OrderByDescending(f => f.GetValueOrDefault(etiqueta)?.ToString() ?? string.Empty).ToList()
                    : filas.OrderBy(f => f.GetValueOrDefault(etiqueta)?.ToString() ?? string.Empty).ToList();
            }

            return filas;
        }
    }
}
