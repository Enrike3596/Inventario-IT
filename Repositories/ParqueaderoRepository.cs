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
        Task<Parqueadero?> ObtenerPorDAAsync(string da);
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
                .Where(p => p.Estado == EstadoGenerico.Activo)
                .ToListAsync();
        }

        public async Task<Parqueadero?> ObtenerPorIdAsync(int id)
        {
            return await _context.Parqueaderos
                .FirstOrDefaultAsync(p => p.IdParqueadero == id);
        }

        public async Task<Parqueadero?> ObtenerPorDAAsync(string da)
        {
            return await _context.Parqueaderos
                .FirstOrDefaultAsync(p => p.DA == da);
        }

        public async Task<Parqueadero> CrearAsync(Parqueadero parqueadero)
        {
            parqueadero.Nombre = (parqueadero.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(parqueadero.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(parqueadero));

            parqueadero.Ubicacion = (parqueadero.Ubicacion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(parqueadero.Ubicacion))
                throw new ArgumentException("Ubicación no puede ser vacía.", nameof(parqueadero));

            parqueadero.DA = (parqueadero.DA ?? string.Empty).Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(parqueadero.DA))
                throw new ArgumentException("DA no puede ser vacío.", nameof(parqueadero));

            var existing = await _context.Parqueaderos.FirstOrDefaultAsync(p => p.DA == parqueadero.DA);
            if (existing != null)
                throw new InvalidOperationException($"Ya existe un parqueadero con el DA '{parqueadero.DA}'.");

            _context.Parqueaderos.Add(parqueadero);

            try
            {
                await _context.SaveChangesAsync();
                return parqueadero;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                                   && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixParqueaderoIdSequenceAsync();
                _context.Entry(parqueadero).State = EntityState.Added;
                await _context.SaveChangesAsync();
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

            var da = (dto.DA ?? string.Empty).Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(da))
                throw new ArgumentException("DA no puede ser vacío.", nameof(dto.DA));

            if (da != parqueadero.DA)
            {
                var existing = await _context.Parqueaderos.FirstOrDefaultAsync(p => p.DA == da);
                if (existing != null)
                    throw new InvalidOperationException($"Ya existe un parqueadero con el DA '{da}'.");
                parqueadero.DA = da;
            }

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            parqueadero.Nombre = nombre;

            var ubicacion = (dto.Ubicacion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(ubicacion))
                throw new ArgumentException("Ubicación no puede ser vacía.", nameof(dto.Ubicacion));
            parqueadero.Ubicacion = ubicacion;

            parqueadero.Estado = dto.Estado;

            parqueadero.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
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
