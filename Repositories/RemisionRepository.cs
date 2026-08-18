using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IRemisionRepository
    {
        Task<List<Remision>> ObtenerTodosAsync();
        Task<Remision?> ObtenerPorIdAsync(int id);
        Task<Remision?> ObtenerConItemsAsync(int id);
        Task<Remision> CrearAsync(Remision remision);
        Task<Remision?> ActualizarAsync(int id, RemisionUpdateDTO dto);
        Task<Remision?> ActualizarDocumentoAsync(int id, string? rutaDocumento, string? nombreDocumento);
        Task<bool> EliminarAsync(int id);
        Task<List<Activos>> ConfirmarIngresoAsync(int idRemision);
    }

    public class RemisionRepository : IRemisionRepository
    {
        private readonly AppDbContext _context;

        public RemisionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Remision>> ObtenerTodosAsync()
        {
            return await _context.Remisiones
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.Categoria)
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.DetallesItem)
                .Where(r => r.Estado)
                .OrderByDescending(r => r.FechaCompra)
                .ToListAsync();
        }

        public async Task<Remision?> ObtenerPorIdAsync(int id)
        {
            return await _context.Remisiones.FindAsync(id);
        }

        public async Task<Remision?> ObtenerConItemsAsync(int id)
        {
            return await _context.Remisiones
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.Categoria)
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.DetallesItem)
                .FirstOrDefaultAsync(r => r.IdRemision == id && r.Estado);
        }

        public async Task<Remision> CrearAsync(Remision remision)
        {
            remision.NumeroRemision = (remision.NumeroRemision ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(remision.NumeroRemision))
                throw new ArgumentException("Número de remisión no puede ser vacío.", nameof(remision));

            remision.Proveedor = (remision.Proveedor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(remision.Proveedor))
                throw new ArgumentException("Proveedor no puede ser vacío.", nameof(remision));

            _context.Remisiones.Add(remision);

            try
            {
                await _context.SaveChangesAsync();
                return remision;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixRemisionIdSequenceAsync();
                _context.Entry(remision).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return remision;
            }
        }

        private async Task FixRemisionIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"Remisiones\"', 'IdRemision'), (SELECT COALESCE(MAX(\"IdRemision\"), 0) FROM \"Remisiones\"));");
        }

        public async Task<Remision?> ActualizarAsync(int id, RemisionUpdateDTO dto)
        {
            var remision = await _context.Remisiones.FindAsync(id);
            if (remision == null) return null;

            var numeroRemision = (dto.NumeroRemision ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(numeroRemision))
                throw new ArgumentException("Número de remisión no puede ser vacío.", nameof(dto.NumeroRemision));
            remision.NumeroRemision = numeroRemision;

            var proveedor = (dto.Proveedor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(proveedor))
                throw new ArgumentException("Proveedor no puede ser vacío.", nameof(dto.Proveedor));
            remision.Proveedor = proveedor;

            remision.RutaDocumento = dto.RutaDocumento;
            remision.NombreDocumento = dto.NombreDocumento;

            remision.MotivoEdicion = (dto.MotivoEdicion ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return remision;
        }

        public async Task<Remision?> ActualizarDocumentoAsync(int id, string? rutaDocumento, string? nombreDocumento)
        {
            var remision = await _context.Remisiones.FindAsync(id);
            if (remision == null) return null;

            remision.RutaDocumento = rutaDocumento;
            remision.NombreDocumento = nombreDocumento;

            await _context.SaveChangesAsync();
            return remision;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var remision = await _context.Remisiones
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.DetallesItem)
                .FirstOrDefaultAsync(r => r.IdRemision == id);
            if (remision == null) return false;

            foreach (var item in remision.ItemsRemision)
            {
                if (item.DetallesItem.Any(d => d.Procesado))
                    throw new InvalidOperationException("No se puede eliminar una remisión con ítems ya procesados.");
            }

            var detalles = remision.ItemsRemision.SelectMany(i => i.DetallesItem).ToList();

            foreach (var detalle in detalles)
                detalle.Estado = false;

            foreach (var item in remision.ItemsRemision)
                item.Estado = false;

            remision.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Activos>> ConfirmarIngresoAsync(int idRemision)
        {
            var remision = await _context.Remisiones
                .Include(r => r.ItemsRemision)
                    .ThenInclude(i => i.DetallesItem)
                .FirstOrDefaultAsync(r => r.IdRemision == idRemision && r.Estado);

            if (remision == null)
                throw new ArgumentException("Remisión no encontrada.");

            var pendientes = remision.ItemsRemision
                .SelectMany(i => i.DetallesItem.Where(d => !d.Procesado))
                .ToList();

            if (pendientes.Count == 0)
                throw new InvalidOperationException("No hay seriales pendientes por procesar.");

            foreach (var detalle in pendientes)
            {
                detalle.Procesado = true;
            }

            await _context.SaveChangesAsync();
            return new List<Activos>();
        }
    }
}