using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IParqueaderoService
    {
        Task<List<ParqueaderoResponseDTO>> ObtenerTodosAsync();
        Task<ParqueaderoResponseDTO?> ObtenerPorIdAsync(int id);
        Task<ParqueaderoResponseDTO> CrearAsync(ParqueaderoCreateDTO dto);
        Task<ParqueaderoResponseDTO?> ActualizarAsync(int id, ParqueaderoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ParqueaderoService : IParqueaderoService
    {
        private readonly IParqueaderoRepository _repo;

        public ParqueaderoService(IParqueaderoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ParqueaderoResponseDTO>> ObtenerTodosAsync()
        {
            var parqueaderos = await _repo.ObtenerTodosAsync();
            return parqueaderos.Select(MapToDTO).ToList();
        }

        public async Task<ParqueaderoResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var parqueadero = await _repo.ObtenerPorIdAsync(id);
            return parqueadero == null ? null : MapToDTO(parqueadero);
        }

        public async Task<ParqueaderoResponseDTO> CrearAsync(ParqueaderoCreateDTO dto)
        {
            var parqueadero = new Parqueadero
            {
                IdSede = dto.IdSede,
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion
            };

            var creado = await _repo.CrearAsync(parqueadero);
            return MapToDTO(creado);
        }

        public async Task<ParqueaderoResponseDTO?> ActualizarAsync(int id, ParqueaderoUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static ParqueaderoResponseDTO MapToDTO(Parqueadero p)
        {
            return new ParqueaderoResponseDTO
            {
                IdParqueadero = p.IdParqueadero,
                IdSede = p.IdSede,
                NombreSede = p.Sede?.Nombre,
                Nombre = p.Nombre,
                Ubicacion = p.Ubicacion,
                Estado = p.Estado
            };
        }
    }
}
