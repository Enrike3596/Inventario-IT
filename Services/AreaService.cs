using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IAreaService
    {
        Task<List<AreaResponseDTO>> ObtenerTodosAsync();
        Task<AreaResponseDTO?> ObtenerPorIdAsync(int id);
        Task<AreaResponseDTO> CrearAsync(AreaCreateDTO dto);
        Task<AreaResponseDTO?> ActualizarAsync(int id, AreaUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _repo;

        public AreaService(IAreaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AreaResponseDTO>> ObtenerTodosAsync()
        {
            var areas = await _repo.ObtenerTodosAsync();
            return areas.Select(MapToDTO).ToList();
        }

        public async Task<AreaResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var area = await _repo.ObtenerPorIdAsync(id);
            return area == null ? null : MapToDTO(area);
        }

        public async Task<AreaResponseDTO> CrearAsync(AreaCreateDTO dto)
        {
            var area = new Area
            {
                NombreArea = dto.NombreArea
            };

            var creado = await _repo.CrearAsync(area);
            return MapToDTO(creado);
        }

        public async Task<AreaResponseDTO?> ActualizarAsync(int id, AreaUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static AreaResponseDTO MapToDTO(Area a)
        {
            return new AreaResponseDTO
            {
                IdArea = a.IdArea,
                NombreArea = a.NombreArea,
                Estado = a.Estado,
                FechaCreacion = a.FechaCreacion,
                FechaModificacion = a.FechaModificacion,
                CreadoPor = a.CreadoPor,
                ModificadoPor = a.ModificadoPor
            };
        }
    }
}
