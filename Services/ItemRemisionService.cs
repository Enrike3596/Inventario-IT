using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IItemRemisionService
    {
        Task<List<ItemRemisionResponseDTO>> ObtenerPorRemisionAsync(int idRemision);
        Task<ItemRemisionResponseDTO?> ObtenerPorIdAsync(int id);
        Task<ItemRemisionResponseDTO> CrearAsync(ItemRemisionCreateDTO dto);
        Task<ItemRemisionResponseDTO?> ActualizarAsync(int id, ItemRemisionUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ItemRemisionService : IItemRemisionService
    {
        private readonly IItemRemisionRepository _repo;

        public ItemRemisionService(IItemRemisionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ItemRemisionResponseDTO>> ObtenerPorRemisionAsync(int idRemision)
        {
            var items = await _repo.ObtenerPorRemisionAsync(idRemision);
            return items.Select(MapToDTO).ToList();
        }

        public async Task<ItemRemisionResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var item = await _repo.ObtenerPorIdAsync(id);
            return item == null ? null : MapToDTO(item);
        }

        public async Task<ItemRemisionResponseDTO> CrearAsync(ItemRemisionCreateDTO dto)
        {
            var item = new ItemRemision
            {
                IdRemision = dto.IdRemision,
                IdCategoria = dto.IdCategoria,
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                CantidadEsperada = dto.CantidadEsperada
            };

            var creado = await _repo.CrearAsync(item);
            return MapToDTO(creado);
        }

        public async Task<ItemRemisionResponseDTO?> ActualizarAsync(int id, ItemRemisionUpdateDTO dto)
        {
            var actualizado = await _repo.ActualizarAsync(id, dto);
            return actualizado == null ? null : MapToDTO(actualizado);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static ItemRemisionResponseDTO MapToDTO(ItemRemision i)
        {
            int ingresados = i.DetallesItem?.Count(d => d.Procesado) ?? 0;
            return new ItemRemisionResponseDTO
            {
                IdItemRemision = i.IdItemRemision,
                IdRemision = i.IdRemision,
                IdCategoria = i.IdCategoria,
                NombreCategoria = i.Categoria?.Nombre,
                Marca = i.Marca,
                Modelo = i.Modelo,
                CantidadEsperada = i.CantidadEsperada,
                CantidadIngresada = ingresados,
                Estado = i.Estado,
                FechaCreacion = i.FechaCreacion,
                FechaModificacion = i.FechaModificacion,
                CreadoPor = i.CreadoPor,
                ModificadoPor = i.ModificadoPor,
                DetallesItem = i.DetallesItem?.Select(d => new DetalleItemRemisionResponseDTO
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
                }).ToList() ?? new()
            };
        }
    }
}