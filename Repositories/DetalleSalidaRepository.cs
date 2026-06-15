using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IDetalleSalidaRepository
    {
        Task<List<DetalleSalida>> ObtenerPorSalidaAsync(int idSalida);
        Task<DetalleSalida?> ObtenerPorIdAsync(int id);
    }

    public class DetalleSalidaRepository : IDetalleSalidaRepository
    {
        private readonly AppDbContext _context;

        public DetalleSalidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleSalida>> ObtenerPorSalidaAsync(int idSalida)
        {
            return await _context.DetallesSalida
                .Include(d => d.Activo)
                .Where(d => d.IdSalida == idSalida)
                .ToListAsync();
        }

        public async Task<DetalleSalida?> ObtenerPorIdAsync(int id)
        {
            return await _context.DetallesSalida
                .Include(d => d.Activo)
                .Include(d => d.Salida)
                .FirstOrDefaultAsync(d => d.IdDetalleSalida == id);
        }
    }
}
