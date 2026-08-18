using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IDetalleItemRemisionService
    {
        Task<List<DetalleItemRemisionResponseDTO>> ObtenerPorItemAsync(int idItemRemision);
        Task<DetalleItemRemisionResponseDTO?> ObtenerPorIdAsync(int id);
        Task<DetalleItemRemisionResponseDTO> CrearAsync(DetalleItemRemisionCreateDTO dto);
        Task<List<DetalleItemRemisionResponseDTO>> CrearBatchAsync(int idItemRemision, List<string> seriales);
        Task<DetalleItemRemisionResponseDTO?> ActualizarAsync(int id, DetalleItemRemisionUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class DetalleItemRemisionService : IDetalleItemRemisionService
    {
        private readonly IDetalleItemRemisionRepository _repo;

        public DetalleItemRemisionService(IDetalleItemRemisionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DetalleItemRemisionResponseDTO>> ObtenerPorItemAsync(int idItemRemision)
        {
            var detalles = await _repo.ObtenerPorItemAsync(idItemRemision);
            return detalles.Select(MapToDTO).ToList();
        }

        public async Task<DetalleItemRemisionResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var detalle = await _repo.ObtenerPorIdAsync(id);
            return detalle == null ? null : MapToDTO(detalle);
        }

        public async Task<DetalleItemRemisionResponseDTO> CrearAsync(DetalleItemRemisionCreateDTO dto)
        {
            var detalle = new DetalleItemRemision
            {
                IdItemRemision = dto.IdItemRemision,
                Serial = dto.Serial,
                Observaciones = dto.Observaciones
            };

            var creado = await _repo.CrearAsync(detalle);
            return MapToDTO(creado);
        }

        public async Task<List<DetalleItemRemisionResponseDTO>> CrearBatchAsync(int idItemRemision, List<string> seriales)
        {
            var creados = await _repo.CrearBatchAsync(idItemRemision, seriales);
            return creados.Select(MapToDTO).ToList();
        }

        public async Task<DetalleItemRemisionResponseDTO?> ActualizarAsync(int id, DetalleItemRemisionUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static DetalleItemRemisionResponseDTO MapToDTO(DetalleItemRemision d)
        {
            return new DetalleItemRemisionResponseDTO
            {
                IdDetalleItemRemision = d.IdDetalleItemRemision,
                IdItemRemision = d.IdItemRemision,
                Serial = d.Serial,
                Procesado = d.Procesado,
                Estado = d.Estado,
                IdActivo = d.IdActivo,
                CodigoActivo = d.Activo?.CodigoActivo,
                Observaciones = d.Observaciones,
                FechaCreacion = d.FechaCreacion,
                FechaModificacion = d.FechaModificacion,
                CreadoPor = d.CreadoPor,
                ModificadoPor = d.ModificadoPor
            };
        }
    }
}