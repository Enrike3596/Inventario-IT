using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface ICategoriaActivoService
    {
        Task<List<CategoriaActivoResponseDTO>> ObtenerTodosAsync();
        Task<CategoriaActivoResponseDTO?> ObtenerPorIdAsync(int id);
        Task<CategoriaActivoResponseDTO> CrearAsync(CategoriaActivoCreateDTO dto);
        Task<CategoriaActivoResponseDTO?> ActualizarAsync(int id, CategoriaActivoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class CategoriaActivoService : ICategoriaActivoService
    {
        private readonly ICategoriaActivoRepository _repo;

        public CategoriaActivoService(ICategoriaActivoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CategoriaActivoResponseDTO>> ObtenerTodosAsync()
        {
            var categorias = await _repo.ObtenerTodosAsync();
            return categorias.Select(MapToDTO).ToList();
        }

        public async Task<CategoriaActivoResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var categoria = await _repo.ObtenerPorIdAsync(id);
            return categoria == null ? null : MapToDTO(categoria);
        }

        public async Task<CategoriaActivoResponseDTO> CrearAsync(CategoriaActivoCreateDTO dto)
        {
            var categoria = new CategoriaActivo
            {
                Nombre = dto.Nombre
            };

            var creada = await _repo.CrearAsync(categoria);
            return MapToDTO(creada);
        }

        public async Task<CategoriaActivoResponseDTO?> ActualizarAsync(int id, CategoriaActivoUpdateDTO dto)
        {
            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static CategoriaActivoResponseDTO MapToDTO(CategoriaActivo c)
        {
            return new CategoriaActivoResponseDTO
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Estado = c.Estado
            };
        }
    }
}
