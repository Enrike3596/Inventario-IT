using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IRolService
    {
        Task<List<RolResponseDTO>> ObtenerTodosAsync();
        Task<RolResponseDTO?> ObtenerPorIdAsync(int id);
        Task<RolResponseDTO> CrearAsync(RolCreateDTO dto);
        Task<RolResponseDTO?> ActualizarAsync(int id, RolUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class RolService : IRolService
    {
        private readonly IRolRepository _repo;

        public RolService(IRolRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RolResponseDTO>> ObtenerTodosAsync()
        {
            var roles = await _repo.ObtenerTodosAsync();
            return roles.Select(MapToDTO).ToList();
        }

        public async Task<RolResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var rol = await _repo.ObtenerPorIdAsync(id);
            return rol == null ? null : MapToDTO(rol);
        }

        public async Task<RolResponseDTO> CrearAsync(RolCreateDTO dto)
        {
            var rol = new Roles
            {
                Nombre = dto.Nombre,
                Tipo = dto.Tipo
            };

            var creado = await _repo.CrearAsync(rol);
            return MapToDTO(creado);
        }

        public async Task<RolResponseDTO?> ActualizarAsync(int id, RolUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static RolResponseDTO MapToDTO(Roles r)
        {
            return new RolResponseDTO
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Tipo = r.Tipo,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaModificacion = r.FechaModificacion,
                CreadoPor = r.CreadoPor,
                ModificadoPor = r.ModificadoPor
            };
        }
    }
}
