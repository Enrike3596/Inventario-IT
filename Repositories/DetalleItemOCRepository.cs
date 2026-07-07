using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IDetalleItemOCRepository
    {
        Task<List<DetalleItemOC>> ObtenerPorItemAsync(int idItemOC);
        Task<DetalleItemOC?> ObtenerPorIdAsync(int id);
        Task<DetalleItemOC> CrearAsync(DetalleItemOC detalle);
        Task<List<DetalleItemOC>> CrearBatchAsync(int idItemOC, List<string> seriales);
        Task<DetalleItemOC?> ActualizarAsync(int id, DetalleItemOCUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class DetalleItemOCRepository : IDetalleItemOCRepository
    {
        private readonly AppDbContext _context;

        public DetalleItemOCRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DetalleItemOC>> ObtenerPorItemAsync(int idItemOC)
        {
            return await _context.DetallesItemOC
                .Include(d => d.ItemOC)
                .Include(d => d.Activo)
                .Where(d => d.IdItemOC == idItemOC)
                .OrderBy(d => d.IdDetalleItemOC)
                .ToListAsync();
        }

        public async Task<DetalleItemOC?> ObtenerPorIdAsync(int id)
        {
            return await _context.DetallesItemOC
                .Include(d => d.ItemOC)
                .Include(d => d.Activo)
                .FirstOrDefaultAsync(d => d.IdDetalleItemOC == id);
        }

        public async Task<DetalleItemOC> CrearAsync(DetalleItemOC detalle)
        {
            var serial = (detalle.Serial ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(serial))
                throw new ArgumentException("Serial no puede ser vacío.", nameof(detalle));

            if (await _context.DetallesItemOC.AnyAsync(d => d.Serial.ToLower() == serial.ToLower()))
                throw new InvalidOperationException($"El serial '{serial}' ya fue registrado en esta orden.");

            detalle.Serial = serial;
            detalle.Observaciones = (detalle.Observaciones ?? string.Empty).Trim();

            _context.DetallesItemOC.Add(detalle);
            await _context.SaveChangesAsync();
            return detalle;
        }

        public async Task<List<DetalleItemOC>> CrearBatchAsync(int idItemOC, List<string> seriales)
        {
            var item = await _context.ItemsOC.FindAsync(idItemOC);
            if (item == null)
                throw new ArgumentException("El item de OC no existe.");

            var existentes = await _context.DetallesItemOC
                .Where(d => d.IdItemOC == idItemOC)
                .Select(d => d.Serial.ToLower())
                .ToListAsync();

            var nuevos = new List<DetalleItemOC>();
            foreach (var s in seriales)
            {
                var serial = (s ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(serial)) continue;

                if (existentes.Contains(serial.ToLower()))
                    throw new InvalidOperationException($"El serial '{serial}' ya fue registrado.");

                var detalle = new DetalleItemOC
                {
                    IdItemOC = idItemOC,
                    Serial = serial
                };
                _context.DetallesItemOC.Add(detalle);
                nuevos.Add(detalle);
                existentes.Add(serial.ToLower());
            }

            await _context.SaveChangesAsync();
            return nuevos;
        }

        public async Task<DetalleItemOC?> ActualizarAsync(int id, DetalleItemOCUpdateDTO dto)
        {
            var detalle = await _context.DetallesItemOC.FindAsync(id);
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
            var detalle = await _context.DetallesItemOC.FindAsync(id);
            if (detalle == null) return false;

            if (detalle.Procesado)
                throw new InvalidOperationException("No se puede eliminar un serial ya procesado.");

            _context.DetallesItemOC.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
