using DTOs;
using Enums;
using Models;
using Repositories;

namespace Services
{
    public interface IHistorialActivoService
    {
        Task<List<HistorialActivoResponseDTO>> ObtenerTodosAsync();
        Task<List<HistorialActivoResponseDTO>> ObtenerPorActivoAsync(int idActivo);
    }

    public class HistorialActivoService : IHistorialActivoService
    {
        private readonly IHistorialActivoRepository _repo;
        private readonly IAsignacionUsuarioRepository _asignacionRepo;

        public HistorialActivoService(IHistorialActivoRepository repo, IAsignacionUsuarioRepository asignacionRepo)
        {
            _repo = repo;
            _asignacionRepo = asignacionRepo;
        }

        public async Task<List<HistorialActivoResponseDTO>> ObtenerTodosAsync()
        {
            var historial = await _repo.ObtenerTodosAsync();
            return await MapToDTOListAsync(historial);
        }

        public async Task<List<HistorialActivoResponseDTO>> ObtenerPorActivoAsync(int idActivo)
        {
            var historial = await _repo.ObtenerPorActivoAsync(idActivo);
            return await MapToDTOListAsync(historial);
        }

        private async Task<List<HistorialActivoResponseDTO>> MapToDTOListAsync(List<Models.HistorialActivo> historial)
        {
            var asignacionIds = historial
                .Where(h => h.IdAsignacion.HasValue)
                .Select(h => h.IdAsignacion!.Value)
                .Distinct()
                .ToList();

            var asignacionesLookup = new Dictionary<int, AsignacionUsuario>();
            if (asignacionIds.Count != 0)
            {
                var asignaciones = await _asignacionRepo.ObtenerPorIdsAsync(asignacionIds);
                foreach (var a in asignaciones)
                    asignacionesLookup[a.IdAsignacion] = a;
            }

            var activoIds = historial
                .Where(h => (h.TipoMovimiento == TipoMovimiento.Asignacion || h.TipoMovimiento == TipoMovimiento.Devolucion)
                            && (!h.IdAsignacion.HasValue || !asignacionesLookup.ContainsKey(h.IdAsignacion!.Value)))
                .Select(h => h.IdActivo)
                .Distinct()
                .ToList();

            var asignacionesPorActivo = new Dictionary<int, AsignacionUsuario>();
            foreach (var activoId in activoIds)
            {
                var asignaciones = await _asignacionRepo.ObtenerPorActivoAsync(activoId);
                var activa = asignaciones.FirstOrDefault();
                if (activa != null)
                    asignacionesPorActivo[activoId] = activa;
            }

            return historial.Select(h =>
            {
                var asignacion = ResolveAsignacion(h, asignacionesLookup, asignacionesPorActivo);
                return MapToDTO(h, asignacion);
            }).ToList();
        }

        private static AsignacionUsuario? ResolveAsignacion(
            Models.HistorialActivo h,
            Dictionary<int, AsignacionUsuario> lookup,
            Dictionary<int, AsignacionUsuario> fallback)
        {
            if (h.IdAsignacion.HasValue && lookup.TryGetValue(h.IdAsignacion.Value, out var a))
                return a;
            if (h.TipoMovimiento == TipoMovimiento.Asignacion || h.TipoMovimiento == TipoMovimiento.Devolucion)
            {
                if (fallback.TryGetValue(h.IdActivo, out var fb))
                    return fb;
            }
            return null;
        }

        private static HistorialActivoResponseDTO MapToDTO(Models.HistorialActivo h, AsignacionUsuario? asignacion)
        {
            return new HistorialActivoResponseDTO
            {
                IdHistorial = h.IdHistorial,
                IdActivo = h.IdActivo,
                CodigoActivo = h.Activo?.CodigoActivo,
                Serial = h.Activo?.Serial,
                IdSalida = h.IdSalida,
                CodigoSalida = h.Salida?.CodigoUnico,
                EstadoActivoSalida = h.Salida != null ? h.Salida.EstadoActivo.ToString() : null,
                Observaciones = h.Observaciones ?? h.Salida?.Observaciones,
                TipoMovimiento = h.TipoMovimiento,
                FechaMovimiento = h.FechaMovimiento,
                IdUsuarioEntrega = h.IdUsuarioEntrega,
                NombreUsuarioEntrega = h.UsuarioEntrega?.Nombre,
                IdAsignacion = h.IdAsignacion,
                NombreUsuarioAsignado = asignacion?.Usuario?.Nombre,
                RegistroSalidaAsignacion = asignacion?.RegistroSalida,
                EstadoAnterior = h.EstadoAnterior,
                EstadoNuevo = h.EstadoNuevo,
                FechaCreacion = h.FechaCreacion,
                FechaModificacion = h.FechaModificacion,
                CreadoPor = h.CreadoPor,
                ModificadoPor = h.ModificadoPor
            };
        }
    }
}
