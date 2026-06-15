using DTOs;
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

        public HistorialActivoService(IHistorialActivoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<HistorialActivoResponseDTO>> ObtenerTodosAsync()
        {
            var historial = await _repo.ObtenerTodosAsync();
            return historial.Select(MapToDTO).ToList();
        }

        public async Task<List<HistorialActivoResponseDTO>> ObtenerPorActivoAsync(int idActivo)
        {
            var historial = await _repo.ObtenerPorActivoAsync(idActivo);
            return historial.Select(MapToDTO).ToList();
        }

        private static HistorialActivoResponseDTO MapToDTO(Models.HistorialActivo h)
        {
            return new HistorialActivoResponseDTO
            {
                IdHistorial = h.IdHistorial,
                IdActivo = h.IdActivo,
                CodigoActivo = h.Activo?.CodigoActivo,
                Serial = h.Activo?.Serial,
                IdSalida = h.IdSalida,
                CodigoSalida = h.Salida?.CodigoUnico,
                TipoMovimiento = h.TipoMovimiento,
                FechaMovimiento = h.FechaMovimiento,
                IdUsuarioEntrega = h.IdUsuarioEntrega,
                NombreUsuarioEntrega = h.UsuarioEntrega?.Nombre
            };
        }
    }
}
