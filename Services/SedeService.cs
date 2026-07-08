using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface ISedeService
    {
        Task<List<SedeResponseDTO>> ObtenerTodosAsync();
        Task<SedeResponseDTO?> ObtenerPorIdAsync(int id);
        Task<SedeResponseDTO> CrearAsync(SedeCreateDTO dto);
        Task<SedeResponseDTO?> ActualizarAsync(int id, SedeUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class SedeService : ISedeService
    {
        private readonly ISedeRepository _repo;

        public SedeService(ISedeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SedeResponseDTO>> ObtenerTodosAsync()
        {
            var sedes = await _repo.ObtenerTodosAsync();
            return sedes.Select(MapToDTO).ToList();
        }

        public async Task<SedeResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var sede = await _repo.ObtenerPorIdAsync(id);
            return sede == null ? null : MapToDTO(sede);
        }

        public async Task<SedeResponseDTO> CrearAsync(SedeCreateDTO dto)
        {
            var sede = new Sedes
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion,
                Ciudad = dto.Ciudad
            };

            var creada = await _repo.CrearAsync(sede);
            return MapToDTO(creada);
        }

        public async Task<SedeResponseDTO?> ActualizarAsync(int id, SedeUpdateDTO dto)
        {
            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static SedeResponseDTO MapToDTO(Sedes s)
        {
            return new SedeResponseDTO
            {
                IdSede = s.IdSede,
                Nombre = s.Nombre,
                Direccion = s.Direccion,
                Ciudad = s.Ciudad,
                Estado = s.Estado,
                FechaCreacion = s.FechaCreacion,
                FechaModificacion = s.FechaModificacion,
                CreadoPor = s.CreadoPor,
                ModificadoPor = s.ModificadoPor
            };
        }
    }
}
