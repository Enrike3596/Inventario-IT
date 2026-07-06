using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuarios>> ObtenerTodosAsync();
        Task<Usuarios?> ObtenerPorIdAsync(int id);
        Task<Usuarios?> ObtenerPorCorreoAsync(string correo);
        Task<Usuarios> CrearAsync(Usuarios usuario);
        Task<Usuarios?> ActualizarAsync(int id, UsuarioUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<bool> ActualizarContrasenaAsync(int id, string nuevaContrasenaHash);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuarios>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sede)
                .Where(u => u.EstadoUsuario == EstadoUsuario.Activo)
                .ToListAsync();
        }

        public async Task<Usuarios?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sede)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<Usuarios?> ObtenerPorCorreoAsync(string correo)
        {
            var normalizedCorreo = (correo ?? string.Empty).Trim().ToLowerInvariant();
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sede)
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == normalizedCorreo);
        }

        public async Task<Usuarios> CrearAsync(Usuarios usuario)
        {
            usuario.Nombre = (usuario.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(usuario));

            usuario.Correo = (usuario.Correo ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new ArgumentException("Correo no puede ser vacío.", nameof(usuario));

            usuario.Telefono = (usuario.Telefono ?? string.Empty).Trim();

            usuario.Cargo = (usuario.Cargo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(usuario.Cargo))
                throw new ArgumentException("Cargo no puede ser vacío.", nameof(usuario));

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();
                await _context.Entry(usuario).Reference(u => u.Sede).LoadAsync();
                return usuario;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixUsuarioIdSequenceAsync();
                _context.Entry(usuario).State = EntityState.Added;
                await _context.SaveChangesAsync();
                await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();
                await _context.Entry(usuario).Reference(u => u.Sede).LoadAsync();
                return usuario;
            }
        }

        private async Task FixUsuarioIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Usuarios\"', 'IdUsuario'), (SELECT COALESCE(MAX(\"IdUsuario\"), 0) FROM \"Usuarios\"));");
        }

        public async Task<Usuarios?> ActualizarAsync(int id, UsuarioUpdateDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            usuario.Nombre = nombre;

            usuario.Correo = (dto.Correo ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new ArgumentException("Correo no puede ser vacío.", nameof(dto.Correo));

            usuario.Telefono = (dto.Telefono ?? string.Empty).Trim();

            var cargo = (dto.Cargo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(cargo))
                throw new ArgumentException("Cargo no puede ser vacío.", nameof(dto.Cargo));
            usuario.Cargo = cargo;

            if (dto.IdRol != usuario.IdRol)
                usuario.IdRol = dto.IdRol;

            if (dto.IdSede != usuario.IdSede)
                usuario.IdSede = dto.IdSede;

            usuario.EstadoUsuario = dto.EstadoUsuario;

            await _context.SaveChangesAsync();
            await _context.Entry(usuario).Reference(u => u.Rol).LoadAsync();
            await _context.Entry(usuario).Reference(u => u.Sede).LoadAsync();
            return usuario;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.EstadoUsuario = EstadoUsuario.Inactivo;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarContrasenaAsync(int id, string nuevaContrasenaHash)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Contraseña = nuevaContrasenaHash;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
