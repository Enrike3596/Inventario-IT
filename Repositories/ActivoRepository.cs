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
                .Include(a => a.OrdenCompra)
                .Include(a => a.ItemOC)
                .Include(a => a.DetalleItemOC)
                .OrderByDescending(a => a.FechaAdquisicion)
                .ToListAsync();
        }

        public async Task<Activos?> ObtenerPorIdAsync(int id)
        {
            return await _context.Activos
                .Include(a => a.Categoria)
                .Include(a => a.OrdenCompra)
                .Include(a => a.ItemOC)
                .Include(a => a.DetalleItemOC)
                .FirstOrDefaultAsync(a => a.IdActivo == id);
        }

        public async Task<Activos?> ObtenerPorSerialAsync(string serial)
        {
            var normalized = (serial ?? string.Empty).Trim().ToLowerInvariant();
            return await _context.Activos
                .Include(a => a.Categoria)
                .Include(a => a.OrdenCompra)
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
                await _context.Entry(activo).Reference(a => a.OrdenCompra).LoadAsync();
                return activo;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixActivoIdSequenceAsync();
                _context.Entry(activo).State = EntityState.Added;
                await _context.SaveChangesAsync();
                await _context.Entry(activo).Reference(a => a.Categoria).LoadAsync();
                await _context.Entry(activo).Reference(a => a.OrdenCompra).LoadAsync();
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

            if (dto.IdCategoria != activo.IdCategoria)
                activo.IdCategoria = dto.IdCategoria;

            if (dto.IdOrden != activo.IdOrden)
                activo.IdOrden = dto.IdOrden;

            var serial = (dto.Serial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(serial))
                throw new ArgumentException("Serial no puede ser vacío.", nameof(dto.Serial));
            activo.Serial = serial;

            var marca = (dto.Marca ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("Marca no puede ser vacía.", nameof(dto.Marca));
            activo.Marca = marca;

            var modelo = (dto.Modelo ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("Modelo no puede ser vacío.", nameof(dto.Modelo));
            activo.Modelo = modelo;

            activo.Referencia = (dto.Referencia ?? string.Empty).Trim();
            activo.EstadoActivo = dto.EstadoActivo;
            activo.FechaBaja = dto.FechaBaja;
            activo.Observaciones = dto.Observaciones;

            activo.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

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
            await _context.Entry(activo).Reference(a => a.OrdenCompra).LoadAsync();
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
    }
}
