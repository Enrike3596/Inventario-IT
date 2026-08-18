using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IDetalleItemRemisionRepository
    {
        Task<List<DetalleItemRemision>> ObtenerPorItemAsync(int idItemRemision);
        Task<DetalleItemRemision?> ObtenerPorIdAsync(int id);
        Task<DetalleItemRemision> CrearAsync(DetalleItemRemision detalle);
        Task<List<DetalleItemRemision>> CrearBatchAsync(int idItemRemision, List<string> seriales);
        Task<DetalleItemRemision?> ActualizarAsync(int id, DetalleItemRemisionUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class DetalleItemRemisionRepository : IDetalleItemRemisionRepository
    {
        private readonly AppDbContext _context;

        public DetalleItemRemisionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleItemRemision>> ObtenerPorItemAsync(int idItemRemision)
        {
            return await _context.DetallesItemRemision
                .Include(d => d.ItemRemision)
                .Include(d => d.Activo)
                .Where(d => d.IdItemRemision == idItemRemision && d.Estado)
                .OrderBy(d => d.IdDetalleItemRemision)
                .ToListAsync();
        }

        public async Task<DetalleItemRemision?> ObtenerPorIdAsync(int id)
        {
            return await _context.DetallesItemRemision
                .Include(d => d.ItemRemision)
                .Include(d => d.Activo)
                .FirstOrDefaultAsync(d => d.IdDetalleItemRemision == id && d.Estado);
        }

        public async Task<DetalleItemRemision> CrearAsync(DetalleItemRemision detalle)
        {
            var serial = (detalle.Serial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(serial))
                throw new ArgumentException("Serial no puede ser vacío.", nameof(detalle));

            if (await _context.DetallesItemRemision.AnyAsync(d => d.Estado && d.Serial.ToLower() == serial.ToLower()))
                throw new InvalidOperationException($"El serial '{serial}' ya fue registrado en esta remisión.");

            detalle.Serial = serial;
            detalle.Observaciones = (detalle.Observaciones ?? string.Empty).Trim();

            _context.DetallesItemRemision.Add(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<List<DetalleItemRemision>> CrearBatchAsync(int idItemRemision, List<string> seriales)
        {
            var item = await _context.ItemsRemision.FindAsync(idItemRemision);
            if (item == null)
                throw new ArgumentException("El ítem de remisión no existe.");

            var existentes = await _context.DetallesItemRemision
                .Where(d => d.IdItemRemision == idItemRemision && d.Estado)
                .Select(d => d.Serial.ToLower())
                .ToListAsync();

            var nuevos = new List<DetalleItemRemision>();
            foreach (var s in seriales)
            {
                var serial = (s ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(serial)) continue;

                if (existentes.Contains(serial.ToLower()))
                    throw new InvalidOperationException($"El serial '{serial}' ya fue registrado.");

                var detalle = new DetalleItemRemision
                {
                    IdItemRemision = idItemRemision,
                    Serial = serial
                };
                _context.DetallesItemRemision.Add(detalle);
                nuevos.Add(detalle);
                existentes.Add(serial.ToLower());
            }

            await _context.SaveChangesAsync();
            return nuevos;
        }

        public async Task<DetalleItemRemision?> ActualizarAsync(int id, DetalleItemRemisionUpdateDTO dto)
        {
            var detalle = await _context.DetallesItemRemision.FindAsync(id);
            if (detalle == null) return null;

            if (detalle.Procesado)
                throw new InvalidOperationException("No se puede modificar un serial ya procesado.");

            var serial = (dto.Serial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(serial))
                throw new ArgumentException("Serial no puede ser vacío.", nameof(dto.Serial));

            detalle.Serial = serial;
            detalle.Observaciones = (dto.Observaciones ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var detalle = await _context.DetallesItemRemision.FindAsync(id);
            if (detalle == null) return false;

            if (detalle.Procesado)
                throw new InvalidOperationException("No se puede eliminar un serial ya procesado.");

            detalle.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}