using DTOs;
using Enums;
using Models;
using Repositories;

namespace Services
{
    public interface IAsignacionUsuarioService
    {
        Task<List<AsignacionUsuarioResponseDTO>> ObtenerTodosAsync();
        Task<AsignacionUsuarioResponseDTO?> ObtenerPorIdAsync(int id);
        Task<AsignacionUsuarioResponseDTO> CrearAsync(AsignacionUsuarioCreateDTO dto);
        Task<AsignacionUsuarioResponseDTO?> DesactivarAsync(int id);
        Task<bool> EliminarAsync(int id);
    }

    public class AsignacionUsuarioService : IAsignacionUsuarioService
    {
        private readonly IAsignacionUsuarioRepository _repo;

        public AsignacionUsuarioService(IAsignacionUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AsignacionUsuarioResponseDTO>> ObtenerTodosAsync()
        {
            var asignaciones = await _repo.ObtenerTodosAsync();
            return asignaciones.Select(MapToDTO).ToList();
        }

        public async Task<AsignacionUsuarioResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var asignacion = await _repo.ObtenerPorIdAsync(id);
            return asignacion == null ? null : MapToDTO(asignacion);
        }

        public async Task<AsignacionUsuarioResponseDTO> CrearAsync(AsignacionUsuarioCreateDTO dto)
        {
            var asignacion = new AsignacionUsuario
            {
                IdActivo = dto.IdActivo,
                IdUsuarioDestino = dto.IdUsuarioDestino,
                IdParqueadero = dto.IdParqueadero,
                FechaAsignacion = DateTime.Now,
                EstadoAsignacion = EstadoAsignacion.Activa
            };

            var creada = await _repo.CrearAsync(asignacion);
            return MapToDTO(creada);
        }

        public async Task<AsignacionUsuarioResponseDTO?> DesactivarAsync(int id)
        {
            var desactivada = await _repo.DesactivarAsync(id);
            return desactivada == null ? null : MapToDTO(desactivada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static AsignacionUsuarioResponseDTO MapToDTO(AsignacionUsuario a)
        {
            return new AsignacionUsuarioResponseDTO
            {
                IdAsignacion = a.IdAsignacion,
                IdActivo = a.IdActivo,
                CodigoActivo = a.ActivoNav?.CodigoActivo,
                Serial = a.ActivoNav?.Serial,
                IdUsuarioDestino = a.IdUsuarioDestino,
                NombreUsuarioDestino = a.Usuario?.Nombre,
                IdParqueadero = a.IdParqueadero,
                NombreParqueadero = a.Parqueadero?.Nombre,
                FechaAsignacion = a.FechaAsignacion,
                EstadoAsignacion = a.EstadoAsignacion
            };
        }
    }
}
