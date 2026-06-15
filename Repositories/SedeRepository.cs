using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface ISedeRepository
    {
        Task<List<Sedes>> ObtenerTodosAsync();
        Task<Sedes?> ObtenerPorIdAsync(int id);
        Task<Sedes> CrearAsync(Sedes sede);
        Task<Sedes?> ActualizarAsync(int id, SedeUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class SedeRepository : ISedeRepository
    {
        private readonly AppDbContext _context;

        public SedeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sedes>> ObtenerTodosAsync()
        {
            return await _context.Sedes
                .Where(s => s.Estado == EstadoGenerico.Activo)
                .ToListAsync();
        }

        public async Task<Sedes?> ObtenerPorIdAsync(int id)
        {
            return await _context.Sedes.FindAsync(id);
        }

        public async Task<Sedes> CrearAsync(Sedes sede)
        {
            sede.Nombre = (sede.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(sede.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(sede));

            sede.Direccion = (sede.Direccion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(sede.Direccion))
                throw new ArgumentException("Dirección no puede ser vacía.", nameof(sede));

            sede.Ciudad = (sede.Ciudad ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(sede.Ciudad))
                throw new ArgumentException("Ciudad no puede ser vacía.", nameof(sede));

            _context.Sedes.Add(sede);

            try
            {
                await _context.SaveChangesAsync();
                return sede;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixSedeIdSequenceAsync();
                _context.Entry(sede).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return sede;
            }
        }

        private async Task FixSedeIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Sedes\"', 'IdSede'), (SELECT COALESCE(MAX(\"IdSede\"), 0) FROM \"Sedes\"));");
        }

        public async Task<Sedes?> ActualizarAsync(int id, SedeUpdateDTO dto)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null) return null;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            sede.Nombre = nombre;

            var direccion = (dto.Direccion ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(direccion))
                throw new ArgumentException("Dirección no puede ser vacía.", nameof(dto.Direccion));
            sede.Direccion = direccion;

            var ciudad = (dto.Ciudad ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(ciudad))
                throw new ArgumentException("Ciudad no puede ser vacía.", nameof(dto.Ciudad));
            sede.Ciudad = ciudad;

            sede.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return sede;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null) return false;

            sede.Estado = EstadoGenerico.Inactivo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
