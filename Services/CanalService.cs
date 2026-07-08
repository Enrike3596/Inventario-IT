using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface ICanalService
    {
        Task<List<CanalResponseDTO>> ObtenerTodosAsync();
        Task<CanalResponseDTO?> ObtenerPorIdAsync(int id);
        Task<CanalResponseDTO> CrearAsync(CanalCreateDTO dto);
        Task<CanalResponseDTO?> ActualizarAsync(int id, CanalUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class CanalService : ICanalService
    {
        private readonly ICanalRepository _repo;

        public CanalService(ICanalRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CanalResponseDTO>> ObtenerTodosAsync()
        {
            var canales = await _repo.ObtenerTodosAsync();
            return canales.Select(MapToDTO).ToList();
        }

        public async Task<CanalResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var canal = await _repo.ObtenerPorIdAsync(id);
            return canal == null ? null : MapToDTO(canal);
        }

        public async Task<CanalResponseDTO> CrearAsync(CanalCreateDTO dto)
        {
            var canal = new Canal
            {
                Nombre = dto.Nombre
            };

            var creado = await _repo.CrearAsync(canal);
            return MapToDTO(creado);
        }

        public async Task<CanalResponseDTO?> ActualizarAsync(int id, CanalUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static CanalResponseDTO MapToDTO(Canal c)
        {
            return new CanalResponseDTO
            {
                IdCanal = c.IdCanal,
                Nombre = c.Nombre,
                FechaSolicitud = c.FechaSolicitud,
                FechaCreacion = c.FechaCreacion,
                FechaModificacion = c.FechaModificacion,
                CreadoPor = c.CreadoPor,
                ModificadoPor = c.ModificadoPor
            };
        }
    }
}
