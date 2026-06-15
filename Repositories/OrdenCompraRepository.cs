using Data;
using DTOs;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IOrdenCompraRepository
    {
        Task<List<OrdenCompra>> ObtenerTodosAsync();
        Task<OrdenCompra?> ObtenerPorIdAsync(int id);
        Task<OrdenCompra> CrearAsync(OrdenCompra orden);
        Task<OrdenCompra?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class OrdenCompraRepository : IOrdenCompraRepository
    {
        private readonly AppDbContext _context;

        public OrdenCompraRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrdenCompra>> ObtenerTodosAsync()
        {
            return await _context.OrdenesCompra
                .OrderByDescending(o => o.FechaCompra)
                .ToListAsync();
        }

        public async Task<OrdenCompra?> ObtenerPorIdAsync(int id)
        {
            return await _context.OrdenesCompra.FindAsync(id);
        }

        public async Task<OrdenCompra> CrearAsync(OrdenCompra orden)
        {
            orden.NumeroOC = (orden.NumeroOC ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(orden.NumeroOC))
                throw new ArgumentException("Número de OC no puede ser vacío.", nameof(orden));

            orden.Proveedor = (orden.Proveedor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(orden.Proveedor))
                throw new ArgumentException("Proveedor no puede ser vacío.", nameof(orden));

            if (orden.Total <= 0)
                throw new ArgumentException("Total debe ser mayor a 0.", nameof(orden));

            _context.OrdenesCompra.Add(orden);

            try
            {
                await _context.SaveChangesAsync();
                return orden;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixOrdenIdSequenceAsync();
                _context.Entry(orden).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return orden;
            }
        }

        private async Task FixOrdenIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"OrdenesCompra\"', 'IdOrden'), (SELECT COALESCE(MAX(\"IdOrden\"), 0) FROM \"OrdenesCompra\"));");
        }

        public async Task<OrdenCompra?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto)
        {
            var orden = await _context.OrdenesCompra.FindAsync(id);
            if (orden == null) return null;

            var numeroOC = (dto.NumeroOC ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(numeroOC))
                throw new ArgumentException("Número de OC no puede ser vacío.", nameof(dto.NumeroOC));
            orden.NumeroOC = numeroOC;

            var proveedor = (dto.Proveedor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(proveedor))
                throw new ArgumentException("Proveedor no puede ser vacío.", nameof(dto.Proveedor));
            orden.Proveedor = proveedor;

            if (dto.Total <= 0)
                throw new ArgumentException("Total debe ser mayor a 0.", nameof(dto.Total));
            orden.Total = dto.Total;

            orden.Observaciones = (dto.Observaciones ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var orden = await _context.OrdenesCompra.FindAsync(id);
            if (orden == null) return false;

            _context.OrdenesCompra.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
