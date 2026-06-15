using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IHistorialActivoRepository
    {
        Task<List<HistorialActivo>> ObtenerTodosAsync();
        Task<List<HistorialActivo>> ObtenerPorActivoAsync(int idActivo);
    }

    public class HistorialActivoRepository : IHistorialActivoRepository
    {
        private readonly AppDbContext _context;

        public HistorialActivoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HistorialActivo>> ObtenerTodosAsync()
        {
            return await _context.HistorialActivos
                .Include(h => h.Activo)
                .Include(h => h.Salida)
                .Include(h => h.UsuarioEntrega)
                .OrderByDescending(h => h.FechaMovimiento)
                .ToListAsync();
        }

        public async Task<List<HistorialActivo>> ObtenerPorActivoAsync(int idActivo)
        {
            return await _context.HistorialActivos
                .Include(h => h.Activo)
                .Include(h => h.Salida)
                .Include(h => h.UsuarioEntrega)
                .Where(h => h.IdActivo == idActivo)
                .OrderByDescending(h => h.FechaMovimiento)
                .ToListAsync();
        }
    }
}
