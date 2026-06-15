using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface ICanalRepository
    {
        Task<List<Canal>> ObtenerTodosAsync();
        Task<Canal?> ObtenerPorIdAsync(int id);
        Task<Canal> CrearAsync(Canal canal);
        Task<Canal?> ActualizarAsync(int id, CanalUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class CanalRepository : ICanalRepository
    {
        private readonly AppDbContext _context;

        public CanalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Canal>> ObtenerTodosAsync()
        {
            return await _context.Canales.ToListAsync();
        }

        public async Task<Canal?> ObtenerPorIdAsync(int id)
        {
            return await _context.Canales.FindAsync(id);
        }

        public async Task<Canal> CrearAsync(Canal canal)
        {
            canal.Nombre = (canal.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(canal.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(canal));

            _context.Canales.Add(canal);

            try
            {
                await _context.SaveChangesAsync();
                return canal;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixCanalIdSequenceAsync();
                _context.Entry(canal).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return canal;
            }
        }

        private async Task FixCanalIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Canales\"', 'IdCanal'), (SELECT COALESCE(MAX(\"IdCanal\"), 0) FROM \"Canales\"));");
        }

        public async Task<Canal?> ActualizarAsync(int id, CanalUpdateDTO dto)
        {
            var canal = await _context.Canales.FindAsync(id);
            if (canal == null) return null;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            canal.Nombre = nombre;

            await _context.SaveChangesAsync();
            return canal;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var canal = await _context.Canales.FindAsync(id);
            if (canal == null) return false;

            _context.Canales.Remove(canal);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
