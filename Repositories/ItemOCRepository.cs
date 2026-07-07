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
                .Where(i => i.IdOrden == idOrden)
                .OrderBy(i => i.IdItemOC)
                .ToListAsync();
        }

        public async Task<ItemOC?> ObtenerPorIdAsync(int id)
        {
            return await _context.ItemsOC
                .Include(i => i.Categoria)
                .Include(i => i.DetallesItem)
                .FirstOrDefaultAsync(i => i.IdItemOC == id);
        }

        public async Task<ItemOC> CrearAsync(ItemOC item)
        {
            item.NombreProducto = (item.NombreProducto ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(item.NombreProducto))
                throw new ArgumentException("Nombre del producto no puede ser vacío.", nameof(item));

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

            var nombre = (dto.NombreProducto ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre del producto no puede ser vacío.", nameof(dto.NombreProducto));
            item.NombreProducto = nombre;

            var marca = (dto.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(dto.Marca));
            item.Marca = marca;

            var modelo = (dto.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(dto.Modelo));
            item.Modelo = modelo;

            item.Referencia = (dto.Referencia ?? string.Empty).Trim();
            item.Observaciones = (dto.Observaciones ?? string.Empty).Trim();
            item.CantidadEsperada = dto.CantidadEsperada;

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

            _context.DetallesItemOC.RemoveRange(item.DetallesItem);
            _context.ItemsOC.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
