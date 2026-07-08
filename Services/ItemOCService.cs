using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IItemOCService
    {
        Task<List<ItemOCResponseDTO>> ObtenerPorOrdenAsync(int idOrden);
        Task<ItemOCResponseDTO?> ObtenerPorIdAsync(int id);
        Task<ItemOCResponseDTO> CrearAsync(ItemOCCreateDTO dto);
        Task<ItemOCResponseDTO?> ActualizarAsync(int id, ItemOCUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ItemOCService : IItemOCService
    {
        private readonly IItemOCRepository _repo;

        public ItemOCService(IItemOCRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ItemOCResponseDTO>> ObtenerPorOrdenAsync(int idOrden)
        {
            var items = await _repo.ObtenerPorOrdenAsync(idOrden);
            return items.Select(MapToDTO).ToList();
        }

        public async Task<ItemOCResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var item = await _repo.ObtenerPorIdAsync(id);
            return item == null ? null : MapToDTO(item);
        }

        public async Task<ItemOCResponseDTO> CrearAsync(ItemOCCreateDTO dto)
        {
            var item = new ItemOC
            {
                IdOrden = dto.IdOrden,
                IdCategoria = dto.IdCategoria,
                NombreProducto = dto.NombreProducto,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Referencia = dto.Referencia,
                Observaciones = dto.Observaciones,
                CantidadEsperada = dto.CantidadEsperada
            };

            var creado = await _repo.CrearAsync(item);
            return MapToDTO(creado);
        }

        public async Task<ItemOCResponseDTO?> ActualizarAsync(int id, ItemOCUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static ItemOCResponseDTO MapToDTO(ItemOC i)
        {
            int ingresados = i.DetallesItem?.Count(d => d.Procesado) ?? 0;
            return new ItemOCResponseDTO
            {
                IdItemOC = i.IdItemOC,
                IdOrden = i.IdOrden,
                IdCategoria = i.IdCategoria,
                NombreCategoria = i.Categoria?.Nombre,
                NombreProducto = i.NombreProducto,
                Marca = i.Marca,
                Modelo = i.Modelo,
                Referencia = i.Referencia,
                Observaciones = i.Observaciones,
                CantidadEsperada = i.CantidadEsperada,
                CantidadIngresada = ingresados,
                FechaCreacion = i.FechaCreacion,
                FechaModificacion = i.FechaModificacion,
                CreadoPor = i.CreadoPor,
                ModificadoPor = i.ModificadoPor,
                DetallesItem = i.DetallesItem?.Select(d => new DetalleItemOCResponseDTO
                {
                    IdDetalleItemOC = d.IdDetalleItemOC,
                    IdItemOC = d.IdItemOC,
                    Serial = d.Serial,
                    Procesado = d.Procesado,
                    IdActivo = d.IdActivo,
                    CodigoActivo = d.Activo?.CodigoActivo,
                    Observaciones = d.Observaciones,
                    FechaCreacion = d.FechaCreacion,
                    FechaModificacion = d.FechaModificacion,
                    CreadoPor = d.CreadoPor,
                    ModificadoPor = d.ModificadoPor
                }).ToList() ?? new()
            };
        }
    }
}
