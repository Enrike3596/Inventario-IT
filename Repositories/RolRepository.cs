using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IRolRepository
    {
        Task<List<Roles>> ObtenerTodosAsync();
        Task<Roles?> ObtenerPorIdAsync(int id);
        Task<Roles> CrearAsync(Roles rol);
        Task<Roles?> ActualizarAsync(int id, RolUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class RolRepository : IRolRepository
    {
        private readonly AppDbContext _context;

        public RolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Roles>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .Where(r => r.Estado == EstadoGenerico.Activo)
                .ToListAsync();
        }

        public async Task<Roles?> ObtenerPorIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Roles> CrearAsync(Roles rol)
        {
            rol.Nombre = (rol.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(rol.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(rol));

            rol.Tipo = (rol.Tipo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(rol.Tipo))
                throw new ArgumentException("Tipo no puede ser vacío.", nameof(rol));

            _context.Roles.Add(rol);

            try
            {
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixRolIdSequenceAsync();
                _context.Entry(rol).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return rol;
            }
        }

        private async Task FixRolIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Roles\"', 'IdRol'), (SELECT COALESCE(MAX(\"IdRol\"), 0) FROM \"Roles\"));");
        }

        public async Task<Roles?> ActualizarAsync(int id, RolUpdateDTO dto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return null;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            rol.Nombre = nombre;

            var tipo = (dto.Tipo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("Tipo no puede ser vacío.", nameof(dto.Tipo));
            rol.Tipo = tipo;

            rol.Estado = dto.Estado;

            rol.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            rol.Estado = EstadoGenerico.Inactivo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
