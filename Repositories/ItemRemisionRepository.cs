using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IItemRemisionRepository
    {
        Task<List<ItemRemision>> ObtenerPorRemisionAsync(int idRemision);
        Task<ItemRemision?> ObtenerPorIdAsync(int id);
        Task<ItemRemision> CrearAsync(ItemRemision item);
        Task<ItemRemision?> ActualizarAsync(int id, ItemRemisionUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ItemRemisionRepository : IItemRemisionRepository
    {
        private readonly AppDbContext _context;

        public ItemRemisionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemRemision>> ObtenerPorRemisionAsync(int idRemision)
        {
            return await _context.ItemsRemision
                .Include(i => i.Categoria)
                .Include(i => i.DetallesItem)
                .Where(i => i.IdRemision == idRemision && i.Estado)
                .OrderBy(i => i.IdItemRemision)
                .ToListAsync();
        }

        public async Task<ItemRemision?> ObtenerPorIdAsync(int id)
        {
            return await _context.ItemsRemision
                .Include(i => i.Categoria)
                .Include(i => i.DetallesItem)
                .FirstOrDefaultAsync(i => i.IdItemRemision == id && i.Estado);
        }

        public async Task<ItemRemision> CrearAsync(ItemRemision item)
        {
            item.Marca = (item.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(item.Marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(item));

            item.Modelo = (item.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(item.Modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(item));

            _context.ItemsRemision.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<ItemRemision?> ActualizarAsync(int id, ItemRemisionUpdateDTO dto)
        {
            var item = await _context.ItemsRemision.FindAsync(id);
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

            item.CantidadEsperada = dto.CantidadEsperada;

            item.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.ItemsRemision
                .Include(i => i.DetallesItem)
                .FirstOrDefaultAsync(i => i.IdItemRemision == id);
            if (item == null) return false;

            if (item.DetallesItem.Any(d => d.Procesado))
                throw new InvalidOperationException("No se puede eliminar un ítem con seriales ya procesados.");

            foreach (var detalle in item.DetallesItem)
                detalle.Estado = false;

            item.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}