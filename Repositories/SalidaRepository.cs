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
                .Include(s => s.CanalSolicitud)
                .Include(s => s.UsuarioDestino)
                .Include(s => s.ParqueaderoDestino)
                .Include(s => s.UsuarioEntrega)
                .Include(s => s.DetallesSalida)
                    .ThenInclude(d => d.Activo)
                .OrderByDescending(s => s.FechaSalida)
                .ToListAsync();
        }

        public async Task<Salida?> ObtenerPorIdAsync(int id)
        {
            return await _context.Salidas
                .Include(s => s.CanalSolicitud)
                .Include(s => s.UsuarioDestino)
                .Include(s => s.ParqueaderoDestino)
                .Include(s => s.UsuarioEntrega)
                .Include(s => s.DetallesSalida)
                    .ThenInclude(d => d.Activo)
                .FirstOrDefaultAsync(s => s.IdSalida == id);
        }

        public async Task<Salida> CrearAsync(Salida salida, List<DetalleSalida> detalles)
        {
            if (salida.IdUsuarioDestino == null && salida.IdParqueaderoDestino == null)
                throw new ArgumentException("Debe especificar un destino (usuario o parqueadero).");

            salida.CodigoUnico = await GenerarCodigoUnicoAsync();
            salida.FechaSalida = DateTime.UtcNow;
            salida.RegistroSalida = (salida.RegistroSalida ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(salida.RegistroSalida))
                throw new ArgumentException("Registro de salida no puede ser vacío.", nameof(salida));

            _context.Salidas.Add(salida);
            await _context.SaveChangesAsync();

            foreach (var detalle in detalles)
            {
                detalle.IdSalida = salida.IdSalida;
                _context.DetallesSalida.Add(detalle);

                var activo = await _context.Activos.FindAsync(detalle.IdActivo);
                if (activo != null)
                {
                    activo.EstadoActivo = EstadoActivo.Asignado;
                    _context.HistorialActivos.Add(new HistorialActivo
                    {
                        IdActivo = detalle.IdActivo,
                        IdSalida = salida.IdSalida,
                        TipoMovimiento = TipoMovimiento.Salida,
                        FechaMovimiento = DateTime.UtcNow,
                        IdUsuarioEntrega = salida.IdUsuarioEntrega
                    });
                }
            }

            await _context.SaveChangesAsync();

            await _context.Entry(salida).Reference(s => s.CanalSolicitud).LoadAsync();
            await _context.Entry(salida).Reference(s => s.UsuarioEntrega).LoadAsync();
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
            var salida = await _context.Salidas.FindAsync(id);
            if (salida == null) return null;

            salida.NumeroTicket = dto.NumeroTicket ?? salida.NumeroTicket;
            salida.Observaciones = dto.Observaciones ?? salida.Observaciones;

            salida.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
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
                if (activo != null)
                    activo.EstadoActivo = EstadoActivo.Disponible;
            }

            _context.Salidas.Remove(salida);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
