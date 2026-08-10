using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IItemOCRepository
    {
        Task<List<ItemOC>> ObtenerPorOrdenAsync(int idOrden);
        Task<ItemOC?> ObtenerPorIdAsync(int id);
        Task<ItemOC> CrearAsync(ItemOC item);
        Task<ItemOC?> ActualizarAsync(int id, ItemOCUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ItemOCRepository : IItemOCRepository
    {
        private readonly AppDbContext _context;

        public ItemOCRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemOC>> ObtenerPorOrdenAsync(int idOrden)
        {
            return await _context.ItemsOC
                .Include(i => i.Categoria)
                .Include(i => i.DetallesItem)
                .Where(i => i.IdOrden == idOrden && i.Estado)
                .OrderBy(i => i.IdItemOC)
                .ToListAsync();
        }

        public async Task<ItemOC?> ObtenerPorIdAsync(int id)
        {
            return await _context.ItemsOC
                .Include(i => i.Categoria)
                .Include(i => i.DetallesItem)
                .FirstOrDefaultAsync(i => i.IdItemOC == id && i.Estado);
        }

        public async Task<ItemOC> CrearAsync(ItemOC item)
        {
            item.Marca = (item.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(item.Marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(item));

            item.Modelo = (item.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(item.Modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(item));

            _context.ItemsOC.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<ItemOC?> ActualizarAsync(int id, ItemOCUpdateDTO dto)
        {
            var item = await _context.ItemsOC.FindAsync(id);
            if (item == null) return null;

            item.IdCategoria = dto.IdCategoria;

            var marca = (dto.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(dto.Marca));
            item.Marca = marca;

            var modelo = (dto.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(dto.Modelo));
            item.Modelo = modelo;

            item.Observaciones = (dto.Observaciones ?? string.Empty).Trim();
            item.CantidadEsperada = dto.CantidadEsperada;

            item.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.ItemsOC
                .Include(i => i.DetallesItem)
                .FirstOrDefaultAsync(i => i.IdItemOC == id);
            if (item == null) return false;

            if (item.DetallesItem.Any(d => d.Procesado))
                throw new InvalidOperationException("No se puede eliminar un item con seriales ya procesados.");

            foreach (var detalle in item.DetallesItem)
                detalle.Estado = false;

            item.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
