using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IActivoRepository
    {
        Task<List<Activos>> ObtenerTodosAsync();
        Task<Activos?> ObtenerPorIdAsync(int id);
        Task<Activos?> ObtenerPorSerialAsync(string serial);
        Task<Activos> CrearAsync(Activos activo);
        Task<Activos?> ActualizarAsync(int id, ActivoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<Activos?> RegistrarRegresoReparacionAsync(int id, RegresoReparacionDTO dto);
    }

    public class ActivoRepository : IActivoRepository
    {
        private readonly AppDbContext _context;

        public ActivoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activos>> ObtenerTodosAsync()
        {
            return await _context.Activos
                .Include(a => a.Categoria)
                .Include(a => a.Remision)
                .Include(a => a.ItemRemision)
                .Include(a => a.DetalleItemRemision)
                .OrderByDescending(a => a.FechaAdquisicion)
                .ToListAsync();
        }

        public async Task<Activos?> ObtenerPorIdAsync(int id)
        {
            return await _context.Activos
                .Include(a => a.Categoria)
                .Include(a => a.Remision)
                .Include(a => a.ItemRemision)
                .Include(a => a.DetalleItemRemision)
                .FirstOrDefaultAsync(a => a.IdActivo == id);
        }

        public async Task<Activos?> ObtenerPorSerialAsync(string serial)
        {
            var normalized = (serial ?? string.Empty).Trim().ToLowerInvariant();
            return await _context.Activos
                .Include(a => a.Categoria)
                .Include(a => a.Remision)
                .FirstOrDefaultAsync(a => a.Serial.ToLower() == normalized);
        }

        public async Task<Activos> CrearAsync(Activos activo)
        {
            activo.Serial = (activo.Serial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(activo.Serial))
                throw new ArgumentException("Serial no puede ser vacío.", nameof(activo));

            activo.Marca = (activo.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(activo.Marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(activo));

            activo.Modelo = (activo.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(activo.Modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(activo));

            // Marcar DetalleItemRemision como procesado si viene vinculado
            if (activo.IdDetalleItemRemision.HasValue)
            {
                var detalle = await _context.DetallesItemRemision.FindAsync(activo.IdDetalleItemRemision.Value);
                if (detalle != null && !detalle.Procesado)
                {
                    detalle.Procesado = true;
                    detalle.Activo = activo;
                }
            }

            _context.Activos.Add(activo);

            try
            {
                await _context.SaveChangesAsync();

                _context.HistorialActivos.Add(new HistorialActivo
                {
                    IdActivo = activo.IdActivo,
                    TipoMovimiento = TipoMovimiento.Entrada,
                    FechaMovimiento = DateTime.UtcNow,
                    EstadoNuevo = EstadoActivo.Disponible.ToString()
                });
                await _context.SaveChangesAsync();

await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();
            await _context.Entry(activo).Reference(a => a.Remision).LoadAsync();
            return activo;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixActivoIdSequenceAsync();
                _context.Entry(activo).State = EntityState.Added;
                await _context.SaveChangesAsync();
                await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();
                await _context.Entry(activo).Reference(a => a.Remision).LoadAsync();
                return activo;
            }
        }

        private async Task FixActivoIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Activos\"', 'IdActivo'), (SELECT COALESCE(MAX(\"IdActivo\"), 0) FROM \"Activos\"));");
        }

        public async Task<Activos?> ActualizarAsync(int id, ActivoUpdateDTO dto)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return null;

            var estadoAnterior = activo.EstadoActivo;

            if (dto.IdCategoria != 0 && dto.IdCategoria != activo.IdCategoria)
                activo.IdCategoria = dto.IdCategoria;

            if (dto.IdRemision != 0 && dto.IdRemision != activo.IdRemision)
                activo.IdRemision = dto.IdRemision;

            if (!string.IsNullOrWhiteSpace(dto.Serial))
                activo.Serial = dto.Serial.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Marca))
                activo.Marca = dto.Marca.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Modelo))
                activo.Modelo = dto.Modelo.Trim();

            activo.EstadoActivo = dto.EstadoActivo;

            if (dto.FechaBaja.HasValue)
                activo.FechaBaja = dto.FechaBaja;

            if (dto.Observaciones != null)
                activo.Observaciones = dto.Observaciones;

            if (dto.MotivoEdicion != null)
                activo.MotivoEdicion = dto.MotivoEdicion.Trim();

            await _context.SaveChangesAsync();

            if (estadoAnterior != dto.EstadoActivo)
            {
                var tipoMovimiento = TipoMovimiento.Entrada;
                if (dto.EstadoActivo == EstadoActivo.EnReparacion)
                    tipoMovimiento = TipoMovimiento.Reparacion;
                else if (dto.EstadoActivo == EstadoActivo.Venta)
                    tipoMovimiento = TipoMovimiento.Salida;
                else if (dto.EstadoActivo == EstadoActivo.DadoDeBaja)
                    tipoMovimiento = TipoMovimiento.Baja;
                else if (dto.EstadoActivo == EstadoActivo.Asignado)
                    tipoMovimiento = TipoMovimiento.Asignacion;
                else if (dto.EstadoActivo == EstadoActivo.Disponible && estadoAnterior != EstadoActivo.Disponible)
                    tipoMovimiento = TipoMovimiento.Devolucion;

                _context.HistorialActivos.Add(new HistorialActivo
                {
                    IdActivo = activo.IdActivo,
                    TipoMovimiento = tipoMovimiento,
                    FechaMovimiento = DateTime.UtcNow,
                    EstadoAnterior = estadoAnterior.ToString(),
                    EstadoNuevo = dto.EstadoActivo.ToString(),
                    Observaciones = dto.Observaciones ?? dto.MotivoEdicion
                });
                await _context.SaveChangesAsync();
            }

await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();
                await _context.Entry(activo).Reference(a => a.Remision).LoadAsync();
                return activo;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return false;

            var estadoAnterior = activo.EstadoActivo;
            activo.EstadoActivo = EstadoActivo.DadoDeBaja;
            activo.FechaBaja = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _context.HistorialActivos.Add(new HistorialActivo
            {
                IdActivo = activo.IdActivo,
                TipoMovimiento = TipoMovimiento.Baja,
                FechaMovimiento = DateTime.UtcNow,
                EstadoAnterior = estadoAnterior.ToString(),
                EstadoNuevo = EstadoActivo.DadoDeBaja.ToString()
            });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Activos?> RegistrarRegresoReparacionAsync(int id, RegresoReparacionDTO dto)
        {
            var activo = await _context.Activos
                .Include(a => a.DetallesSalida)
                    .ThenInclude(ds => ds.Salida)
                .FirstOrDefaultAsync(a => a.IdActivo == id);

            if (activo == null) return null;

            if (activo.EstadoActivo != EstadoActivo.EnReparacion)
                throw new InvalidOperationException("El activo no está en estado 'En reparación'.");

            var estadoAnterior = activo.EstadoActivo;

            // Find the active repair salida for this asset
            var salidaReparacion = activo.DetallesSalida
                .Where(ds => ds.Salida != null && ds.Salida.EstadoActivo == EstadoActivo.EnReparacion && ds.Salida.Estado)
                .Select(ds => ds.Salida)
                .FirstOrDefault();

            // Update asset state to Disponible
            activo.EstadoActivo = EstadoActivo.Disponible;
            activo.FechaModificacion = DateTime.UtcNow;

            // Create Devolucion movement in HistorialActivo
            _context.HistorialActivos.Add(new HistorialActivo
            {
                IdActivo = activo.IdActivo,
                IdSalida = salidaReparacion?.IdSalida,
                TipoMovimiento = TipoMovimiento.Devolucion,
                FechaMovimiento = DateTime.UtcNow,
                EstadoAnterior = estadoAnterior.ToString(),
                EstadoNuevo = EstadoActivo.Disponible.ToString(),
                Observaciones = dto.Observaciones
            });

            // Optionally update the salida to mark it as completed (if needed)
            if (salidaReparacion != null)
            {
                // Check if all assets in this salida are now Disponible
                var allDisponible = await _context.DetallesSalida
                    .Where(ds => ds.IdSalida == salidaReparacion.IdSalida)
                    .AllAsync(ds => _context.Activos
                        .Where(a => a.IdActivo == ds.IdActivo)
                        .Select(a => a.EstadoActivo)
                        .FirstOrDefault() == EstadoActivo.Disponible);

                if (allDisponible)
                {
                    salidaReparacion.Estado = false; // Mark salida as completed/inactive
                    salidaReparacion.FechaModificacion = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();
            await _context.Entry(activo).Reference(a => a.Remision).LoadAsync();

            return activo;
        }
    }
}
