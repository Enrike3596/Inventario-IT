using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface ISalidaRepository
    {
        Task<List<Salida>> ObtenerTodosAsync();
        Task<Salida?> ObtenerPorIdAsync(int id);
        Task<Salida> CrearAsync(Salida salida, List<DetalleSalida> detalles);
        Task<Salida?> ActualizarAsync(int id, SalidaUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class SalidaRepository : ISalidaRepository
    {
        private readonly AppDbContext _context;

        public SalidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Salida>> ObtenerTodosAsync()
        {
            return await _context.Salidas
                .Include(s => s.DetallesSalida)
                    .ThenInclude(d => d.Activo)
                .Where(s => s.Estado)
                .OrderByDescending(s => s.FechaSalida)
                .ToListAsync();
        }

        public async Task<Salida?> ObtenerPorIdAsync(int id)
        {
            return await _context.Salidas
                .Include(s => s.DetallesSalida)
                    .ThenInclude(d => d.Activo)
                .FirstOrDefaultAsync(s => s.IdSalida == id && s.Estado);
        }

        public async Task<Salida> CrearAsync(Salida salida, List<DetalleSalida> detalles)
        {
            salida.CodigoUnico = await GenerarCodigoUnicoAsync();
            salida.FechaSalida = DateTime.UtcNow;

            _context.Salidas.Add(salida);
            await _context.SaveChangesAsync();

            foreach (var detalle in detalles)
            {
                detalle.IdSalida = salida.IdSalida;
                _context.DetallesSalida.Add(detalle);

                var activo = await _context.Activos.FindAsync(detalle.IdActivo);
                if (activo != null)
                {
                    var estadoAnterior = activo.EstadoActivo;
                    activo.EstadoActivo = salida.EstadoActivo;
                    _context.HistorialActivos.Add(new HistorialActivo
                    {
                        IdActivo = detalle.IdActivo,
                        IdSalida = salida.IdSalida,
                        TipoMovimiento = TipoMovimiento.Salida,
                        FechaMovimiento = DateTime.UtcNow,
                        EstadoAnterior = estadoAnterior.ToString(),
                        EstadoNuevo = salida.EstadoActivo.ToString()
                    });
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(salida).Collection(s => s.DetallesSalida).LoadAsync();

            return salida;
        }

        private async Task<string> GenerarCodigoUnicoAsync()
        {
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            var ultimoCodigo = await _context.Salidas
                .Where(s => s.CodigoUnico.StartsWith($"SAL-{fecha}"))
                .OrderByDescending(s => s.CodigoUnico)
                .Select(s => s.CodigoUnico)
                .FirstOrDefaultAsync();

            int correlativo = 1;
            if (ultimoCodigo != null)
            {
                var partes = ultimoCodigo.Split('-');
                if (partes.Length == 3 && int.TryParse(partes[2], out int ultimo))
                    correlativo = ultimo + 1;
            }

            return $"SAL-{fecha}-{correlativo:D6}";
        }

        public async Task<Salida?> ActualizarAsync(int id, SalidaUpdateDTO dto)
        {
            var salida = await _context.Salidas
                .Include(s => s.DetallesSalida)
                .FirstOrDefaultAsync(s => s.IdSalida == id);
            if (salida == null) return null;

            var estadoAnterior = salida.EstadoActivo;
            salida.EstadoActivo = dto.EstadoActivo;
            salida.Observaciones = dto.Observaciones ?? salida.Observaciones;
            salida.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();
            salida.FechaModificacion = DateTime.UtcNow;

            if (estadoAnterior != dto.EstadoActivo)
            {
                foreach (var detalle in salida.DetallesSalida)
                {
                    var activo = await _context.Activos.FindAsync(detalle.IdActivo);
                    if (activo != null)
                    {
                        activo.EstadoActivo = dto.EstadoActivo;
                        _context.HistorialActivos.Add(new HistorialActivo
                        {
                            IdActivo = detalle.IdActivo,
                            IdSalida = salida.IdSalida,
                            TipoMovimiento = TipoMovimiento.Salida,
                            FechaMovimiento = DateTime.UtcNow,
                            EstadoAnterior = estadoAnterior.ToString(),
                            EstadoNuevo = dto.EstadoActivo.ToString()
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(salida).Collection(s => s.DetallesSalida).LoadAsync();

            return salida;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var salida = await _context.Salidas
                .Include(s => s.DetallesSalida)
                .FirstOrDefaultAsync(s => s.IdSalida == id);
            if (salida == null) return false;

            foreach (var detalle in salida.DetallesSalida)
            {
                var activo = await _context.Activos.FindAsync(detalle.IdActivo);
                if (activo != null && activo.EstadoActivo != EstadoActivo.Disponible)
                {
                    var estadoAnterior = activo.EstadoActivo;
                    activo.EstadoActivo = EstadoActivo.Disponible;
                    _context.HistorialActivos.Add(new HistorialActivo
                    {
                        IdActivo = detalle.IdActivo,
                        IdSalida = salida.IdSalida,
                        TipoMovimiento = TipoMovimiento.Devolucion,
                        FechaMovimiento = DateTime.UtcNow,
                        EstadoAnterior = estadoAnterior.ToString(),
                        EstadoNuevo = EstadoActivo.Disponible.ToString(),
                        Observaciones = $"Salida anulada ({salida.CodigoUnico})"
                    });
                }
            }

            salida.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
