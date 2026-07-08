using DTOs;
using Enums;
using Models;
using Repositories;

namespace Services
{
    public interface IActivoService
    {
        Task<List<ActivoResponseDTO>> ObtenerTodosAsync();
        Task<ActivoResponseDTO?> ObtenerPorIdAsync(int id);
        Task<ActivoResponseDTO?> ObtenerPorSerialAsync(string serial);
        Task<ActivoResponseDTO> CrearAsync(ActivoCreateDTO dto);
        Task<ActivoResponseDTO?> ActualizarAsync(int id, ActivoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ActivoService : IActivoService
    {
        private readonly IActivoRepository _repo;

        public ActivoService(IActivoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ActivoResponseDTO>> ObtenerTodosAsync()
        {
            var activos = await _repo.ObtenerTodosAsync();
            return activos.Select(MapToDTO).ToList();
        }

        public async Task<ActivoResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var activo = await _repo.ObtenerPorIdAsync(id);
            return activo == null ? null : MapToDTO(activo);
        }

        public async Task<ActivoResponseDTO?> ObtenerPorSerialAsync(string serial)
        {
            var activo = await _repo.ObtenerPorSerialAsync(serial);
            return activo == null ? null : MapToDTO(activo);
        }

        public async Task<ActivoResponseDTO> CrearAsync(ActivoCreateDTO dto)
        {
            var activo = new Activos
            {
                IdCategoria = dto.IdCategoria,
                IdOrden = dto.IdOrden,
                IdItemOC = dto.IdItemOC,
                IdDetalleItemOC = dto.IdDetalleItemOC,
                CodigoActivo = dto.CodigoActivo ?? string.Empty,
                Serial = dto.Serial,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Referencia = dto.Referencia,
                EstadoActivo = EstadoActivo.Disponible,
                FechaAdquisicion = dto.FechaAdquisicion,
                Observaciones = dto.Observaciones
            };

            var creado = await _repo.CrearAsync(activo);
            return MapToDTO(creado);
        }

        public async Task<ActivoResponseDTO?> ActualizarAsync(int id, ActivoUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static ActivoResponseDTO MapToDTO(Activos a)
        {
            return new ActivoResponseDTO
            {
                IdActivo = a.IdActivo,
                IdCategoria = a.IdCategoria,
                NombreCategoria = a.Categoria?.Nombre,
                IdOrden = a.IdOrden,
                NumeroOC = a.OrdenCompra?.NumeroOC,
                IdItemOC = a.IdItemOC,
                IdDetalleItemOC = a.IdDetalleItemOC,
                CodigoActivo = a.CodigoActivo,
                Serial = a.Serial,
                Marca = a.Marca,
                Modelo = a.Modelo,
                Referencia = a.Referencia,
                EstadoActivo = a.EstadoActivo,
                FechaAdquisicion = a.FechaAdquisicion,
                FechaBaja = a.FechaBaja,
                Observaciones = a.Observaciones,
                FechaCreacion = a.FechaCreacion,
                FechaModificacion = a.FechaModificacion,
                CreadoPor = a.CreadoPor,
                ModificadoPor = a.ModificadoPor
            };
        }
    }
}
