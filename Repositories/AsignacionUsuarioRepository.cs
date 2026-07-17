using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IAsignacionUsuarioRepository
    {
        Task<List<AsignacionUsuario>> ObtenerTodosAsync();
        Task<AsignacionUsuario?> ObtenerPorIdAsync(int id);
        Task<List<AsignacionUsuario>> ObtenerPorActivoAsync(int idActivo);
        Task<List<AsignacionUsuario>> ObtenerPorIdsAsync(List<int> ids);
        Task<AsignacionUsuario> CrearAsync(AsignacionUsuario asignacion);
        Task<AsignacionUsuario?> ActualizarAsync(int id, AsignacionUsuarioUpdateDTO dto);
        Task<AsignacionUsuario?> DesactivarAsync(int id);
        Task<bool> EliminarAsync(int id);
    }

    public class AsignacionUsuarioRepository : IAsignacionUsuarioRepository
    {
        private readonly AppDbContext _context;

        public AsignacionUsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AsignacionUsuario>> ObtenerTodosAsync()
        {
            return await _context.AsignacionesUsuario
                .Include(a => a.ActivoNav)
                .Include(a => a.Usuario)
                .Include(a => a.Parqueadero)
                .Include(a => a.CanalSolicitud)
                .Include(a => a.UsuarioEntrega)
                .Where(a => a.EstadoAsignacion == EstadoAsignacion.Activa)
                .ToListAsync();
        }

        public async Task<AsignacionUsuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.AsignacionesUsuario
                .Include(a => a.ActivoNav)
                .Include(a => a.Usuario)
                .Include(a => a.Parqueadero)
                .Include(a => a.CanalSolicitud)
                .Include(a => a.UsuarioEntrega)
                .FirstOrDefaultAsync(a => a.IdAsignacion == id);
        }

        public async Task<List<AsignacionUsuario>> ObtenerPorActivoAsync(int idActivo)
        {
            return await _context.AsignacionesUsuario
                .Include(a => a.ActivoNav)
                .Include(a => a.Usuario)
                .Include(a => a.Parqueadero)
                .Include(a => a.CanalSolicitud)
                .Include(a => a.UsuarioEntrega)
                .Where(a => a.IdActivo == idActivo)
                .OrderByDescending(a => a.FechaAsignacion)
                .ToListAsync();
        }

        public async Task<List<AsignacionUsuario>> ObtenerPorIdsAsync(List<int> ids)
        {
            return await _context.AsignacionesUsuario
                .Include(a => a.Usuario)
                .Where(a => ids.Contains(a.IdAsignacion))
                .ToListAsync();
        }

        public async Task<AsignacionUsuario> CrearAsync(AsignacionUsuario asignacion)
        {
            var activa = await _context.AsignacionesUsuario
                .AnyAsync(a => a.IdActivo == asignacion.IdActivo && a.EstadoAsignacion == EstadoAsignacion.Activa);
            if (activa)
                throw new InvalidOperationException("El activo ya está asignado actualmente.");

            _context.AsignacionesUsuario.Add(asignacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixAsignacionIdSequenceAsync();
                _context.Entry(asignacion).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }

            var activo = await _context.Activos.FindAsync(asignacion.IdActivo);
            if (activo != null)
            {
                activo.EstadoActivo = EstadoActivo.Asignado;
                _context.HistorialActivos.Add(new HistorialActivo
                {
                    IdActivo = asignacion.IdActivo,
                    IdAsignacion = asignacion.IdAsignacion,
                    TipoMovimiento = TipoMovimiento.Asignacion,
                    FechaMovimiento = DateTime.UtcNow,
                    IdUsuarioEntrega = asignacion.IdUsuarioEntrega,
                    EstadoAnterior = EstadoActivo.Disponible.ToString(),
                    EstadoNuevo = EstadoActivo.Asignado.ToString()
                });
                await _context.SaveChangesAsync();
            }

            await _context.Entry(asignacion).Reference(a => a.ActivoNav).LoadAsync();
            await _context.Entry(asignacion).Reference(a => a.Usuario).LoadAsync();
            await _context.Entry(asignacion).Reference(a => a.CanalSolicitud).LoadAsync();
            await _context.Entry(asignacion).Reference(a => a.UsuarioEntrega).LoadAsync();
            return asignacion;
        }

        private async Task FixAsignacionIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"AsignacionesUsuario\"', 'IdAsignacion'), (SELECT COALESCE(MAX(\"IdAsignacion\"), 0) FROM \"AsignacionesUsuario\"));");
        }

        public async Task<AsignacionUsuario?> ActualizarAsync(int id, AsignacionUsuarioUpdateDTO dto)
        {
            var asignacion = await _context.AsignacionesUsuario
                .Include(a => a.ActivoNav)
                .FirstOrDefaultAsync(a => a.IdAsignacion == id);
            if (asignacion == null) return null;

            var estadoAnterior = asignacion.EstadoAsignacion;
            asignacion.EstadoAsignacion = dto.EstadoAsignacion;
            asignacion.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();

            if (estadoAnterior == EstadoAsignacion.Activa && dto.EstadoAsignacion == EstadoAsignacion.Finalizada)
            {
                var activo = asignacion.ActivoNav;
                if (activo != null && activo.EstadoActivo == EstadoActivo.Asignado)
                {
                    activo.EstadoActivo = EstadoActivo.Disponible;
                    _context.HistorialActivos.Add(new HistorialActivo
                    {
                        IdActivo = asignacion.IdActivo,
                        IdAsignacion = asignacion.IdAsignacion,
                        TipoMovimiento = TipoMovimiento.Devolucion,
                        FechaMovimiento = DateTime.UtcNow,
                        IdUsuarioEntrega = dto.IdUsuarioRecibe ?? asignacion.IdUsuarioEntrega,
                        EstadoAnterior = EstadoActivo.Asignado.ToString(),
                        EstadoNuevo = EstadoActivo.Disponible.ToString()
                    });
                    await _context.SaveChangesAsync();
                }
            }
            else if (estadoAnterior == EstadoAsignacion.Finalizada && dto.EstadoAsignacion == EstadoAsignacion.Activa)
            {
                var activo = asignacion.ActivoNav;
                if (activo != null && activo.EstadoActivo != EstadoActivo.Asignado)
                {
                    activo.EstadoActivo = EstadoActivo.Asignado;
                    _context.HistorialActivos.Add(new HistorialActivo
                    {
                        IdActivo = asignacion.IdActivo,
                        IdAsignacion = asignacion.IdAsignacion,
                        TipoMovimiento = TipoMovimiento.Asignacion,
                        FechaMovimiento = DateTime.UtcNow,
                        IdUsuarioEntrega = dto.IdUsuarioRecibe ?? asignacion.IdUsuarioEntrega,
                        EstadoAnterior = EstadoActivo.Disponible.ToString(),
                        EstadoNuevo = EstadoActivo.Asignado.ToString()
                    });
                    await _context.SaveChangesAsync();
                }
            }

            return asignacion;
        }

        public async Task<AsignacionUsuario?> DesactivarAsync(int id)
        {
            var asignacion = await _context.AsignacionesUsuario
                .Include(a => a.ActivoNav)
                .FirstOrDefaultAsync(a => a.IdAsignacion == id);
            if (asignacion == null) return null;

            if (asignacion.EstadoAsignacion == EstadoAsignacion.Finalizada)
                return asignacion;

            asignacion.EstadoAsignacion = EstadoAsignacion.Finalizada;
            await _context.SaveChangesAsync();

            var activo = asignacion.ActivoNav;
            if (activo != null && activo.EstadoActivo == EstadoActivo.Asignado)
            {
                activo.EstadoActivo = EstadoActivo.Disponible;
                _context.HistorialActivos.Add(new HistorialActivo
                {
                    IdActivo = asignacion.IdActivo,
                    IdAsignacion = asignacion.IdAsignacion,
                    TipoMovimiento = TipoMovimiento.Devolucion,
                    FechaMovimiento = DateTime.UtcNow,
                    IdUsuarioEntrega = asignacion.IdUsuarioEntrega,
                    EstadoAnterior = EstadoActivo.Asignado.ToString(),
                    EstadoNuevo = EstadoActivo.Disponible.ToString()
                });
                await _context.SaveChangesAsync();
            }

            return asignacion;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var asignacion = await _context.AsignacionesUsuario.FindAsync(id);
            if (asignacion == null) return false;

            _context.AsignacionesUsuario.Remove(asignacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
