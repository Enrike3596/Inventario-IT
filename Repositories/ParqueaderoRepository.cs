using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IParqueaderoRepository
    {
        Task<List<Parqueadero>> ObtenerTodosAsync();
        Task<Parqueadero?> ObtenerPorIdAsync(int id);
        Task<Parqueadero> CrearAsync(Parqueadero parqueadero);
        Task<Parqueadero?> ActualizarAsync(int id, ParqueaderoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class ParqueaderoRepository : IParqueaderoRepository
    {
        private readonly AppDbContext _context;

        public ParqueaderoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Parqueadero>> ObtenerTodosAsync()
        {
            return await _context.Parqueaderos
                .Include(p => p.Sede)
                .Where(p => p.Estado == EstadoGenerico.Activo)
                .ToListAsync();
        }

        public async Task<Parqueadero?> ObtenerPorIdAsync(int id)
        {
            return await _context.Parqueaderos
                .Include(p => p.Sede)
                .FirstOrDefaultAsync(p => p.IdParqueadero == id);
        }

        public async Task<Parqueadero> CrearAsync(Parqueadero parqueadero)
        {
            parqueadero.Nombre = (parqueadero.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(parqueadero.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(parqueadero));

            parqueadero.Ubicacion = (parqueadero.Ubicacion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(parqueadero.Ubicacion))
                throw new ArgumentException("Ubicación no puede ser vacía.", nameof(parqueadero));

            _context.Parqueaderos.Add(parqueadero);

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(parqueadero).Reference(p => p.Sede).LoadAsync();
                return parqueadero;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixParqueaderoIdSequenceAsync();
                _context.Entry(parqueadero).State = EntityState.Added;
                await _context.SaveChangesAsync();
                await _context.Entry(parqueadero).Reference(p => p.Sede).LoadAsync();
                return parqueadero;
            }
        }

        private async Task FixParqueaderoIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Parqueaderos\"', 'IdParqueadero'), (SELECT COALESCE(MAX(\"IdParqueadero\"), 0) FROM \"Parqueaderos\"));");
        }

        public async Task<Parqueadero?> ActualizarAsync(int id, ParqueaderoUpdateDTO dto)
        {
            var parqueadero = await _context.Parqueaderos.FindAsync(id);
            if (parqueadero == null) return null;

            if (dto.IdSede != parqueadero.IdSede)
                parqueadero.IdSede = dto.IdSede;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            parqueadero.Nombre = nombre;

            var ubicacion = (dto.Ubicacion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(ubicacion))
                throw new ArgumentException("Ubicación no puede ser vacía.", nameof(dto.Ubicacion));
            parqueadero.Ubicacion = ubicacion;

            parqueadero.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            await _context.Entry(parqueadero).Reference(p => p.Sede).LoadAsync();
            return parqueadero;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var parqueadero = await _context.Parqueaderos.FindAsync(id);
            if (parqueadero == null) return false;

            parqueadero.Estado = EstadoGenerico.Inactivo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
