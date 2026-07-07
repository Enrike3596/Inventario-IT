using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IDetalleItemOCService
    {
        Task<List<DetalleItemOCResponseDTO>> ObtenerPorItemAsync(int idItemOC);
        Task<DetalleItemOCResponseDTO?> ObtenerPorIdAsync(int id);
        Task<DetalleItemOCResponseDTO> CrearAsync(DetalleItemOCCreateDTO dto);
        Task<List<DetalleItemOCResponseDTO>> CrearBatchAsync(int idItemOC, List<string> seriales);
        Task<DetalleItemOCResponseDTO?> ActualizarAsync(int id, DetalleItemOCUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class DetalleItemOCService : IDetalleItemOCService
    {
        private readonly IDetalleItemOCRepository _repo;

        public DetalleItemOCService(IDetalleItemOCRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DetalleItemOCResponseDTO>> ObtenerPorItemAsync(int idItemOC)
        {
            var detalles = await _repo.ObtenerPorItemAsync(idItemOC);
            return detalles.Select(MapToDTO).ToList();
        }

        public async Task<DetalleItemOCResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var detalle = await _repo.ObtenerPorIdAsync(id);
            return detalle == null ? null : MapToDTO(detalle);
        }

        public async Task<DetalleItemOCResponseDTO> CrearAsync(DetalleItemOCCreateDTO dto)
        {
            var detalle = new DetalleItemOC
            {
                IdItemOC = dto.IdItemOC,
                Serial = dto.Serial,
                Observaciones = dto.Observaciones
            };

            var creado = await _repo.CrearAsync(detalle);
            return MapToDTO(creado);
        }

        public async Task<List<DetalleItemOCResponseDTO>> CrearBatchAsync(int idItemOC, List<string> seriales)
        {
            var creados = await _repo.CrearBatchAsync(idItemOC, seriales);
            return creados.Select(MapToDTO).ToList();
        }

        public async Task<DetalleItemOCResponseDTO?> ActualizarAsync(int id, DetalleItemOCUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static DetalleItemOCResponseDTO MapToDTO(DetalleItemOC d)
        {
            return new DetalleItemOCResponseDTO
            {
                IdDetalleItemOC = d.IdDetalleItemOC,
                IdItemOC = d.IdItemOC,
                Serial = d.Serial,
                Procesado = d.Procesado,
                IdActivo = d.IdActivo,
                CodigoActivo = d.Activo?.CodigoActivo,
                Observaciones = d.Observaciones
            };
        }
    }
}
