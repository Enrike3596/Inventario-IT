using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface IOrdenCompraRepository
    {
        Task<List<OrdenCompra>> ObtenerTodosAsync();
        Task<OrdenCompra?> ObtenerPorIdAsync(int id);
        Task<OrdenCompra?> ObtenerConItemsAsync(int id);
        Task<OrdenCompra> CrearAsync(OrdenCompra orden);
        Task<OrdenCompra?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<List<Activos>> ConfirmarIngresoAsync(int idOrden);
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
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.Categoria)
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.DetallesItem)
                .OrderByDescending(o => o.FechaCompra)
                .ToListAsync();
        }

        public async Task<OrdenCompra?> ObtenerPorIdAsync(int id)
        {
            return await _context.OrdenesCompra.FindAsync(id);
        }

        public async Task<OrdenCompra?> ObtenerConItemsAsync(int id)
        {
            return await _context.OrdenesCompra
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.Categoria)
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.DetallesItem)
                .FirstOrDefaultAsync(o => o.IdOrden == id);
        }

        public async Task<OrdenCompra> CrearAsync(OrdenCompra orden)
        {
            orden.NumeroOC = (orden.NumeroOC ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(orden.NumeroOC))
                throw new ArgumentException("Número de OC no puede ser vacío.", nameof(orden));

            orden.Proveedor = (orden.Proveedor ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(orden.Proveedor))
                throw new ArgumentException("Proveedor no puede ser vacío.", nameof(orden));

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

            orden.Total = dto.Total;
            orden.Observaciones = (dto.Observaciones ?? string.Empty).Trim();

            await _context.SaveChangesAsync();
            return orden;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.DetallesItem)
                .FirstOrDefaultAsync(o => o.IdOrden == id);
            if (orden == null) return false;

            foreach (var item in orden.ItemsOC)
            {
                if (item.DetallesItem.Any(d => d.Procesado))
                    throw new InvalidOperationException("No se puede eliminar una orden con items ya procesados.");
            }

            var detalles = orden.ItemsOC.SelectMany(i => i.DetallesItem).ToList();
            _context.DetallesItemOC.RemoveRange(detalles);
            _context.ItemsOC.RemoveRange(orden.ItemsOC);
            _context.OrdenesCompra.Remove(orden);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Activos>> ConfirmarIngresoAsync(int idOrden)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.DetallesItem)
                .Include(o => o.ItemsOC)
                    .ThenInclude(i => i.Categoria)
                .FirstOrDefaultAsync(o => o.IdOrden == idOrden);

            if (orden == null)
                throw new ArgumentException("Orden de compra no encontrada.");

            var activosCreados = new List<Activos>();
            var now = DateTime.UtcNow;
            var nextCodigo = await _context.Activos.MaxAsync(a => (int?)a.IdActivo) ?? 0;

            foreach (var item in orden.ItemsOC)
            {
                foreach (var detalle in item.DetallesItem.Where(d => !d.Procesado))
                {
                    nextCodigo++;
                    var activo = new Activos
                    {
                        IdCategoria = item.IdCategoria,
                        IdOrden = orden.IdOrden,
                        IdItemOC = item.IdItemOC,
                        IdDetalleItemOC = detalle.IdDetalleItemOC,
                        CodigoActivo = $"ACT-{nextCodigo:D4}",
                        Serial = detalle.Serial,
                        Marca = item.Marca,
                        Modelo = item.Modelo,
                        Referencia = item.Referencia,
                        EstadoActivo = EstadoActivo.Disponible,
                        FechaAdquisicion = now,
                        Observaciones = detalle.Observaciones
                    };

                    _context.Activos.Add(activo);
                    detalle.Procesado = true;
                    detalle.IdActivo = activo.IdActivo;
                    activosCreados.Add(activo);
                }
            }

            if (activosCreados.Count == 0)
                throw new InvalidOperationException("No hay seriales pendientes por procesar.");

            await _context.SaveChangesAsync();

            // Cargar relaciones
            foreach (var a in activosCreados)
            {
                await _context.Entry(a).Reference(aa => aa.Categoria).LoadAsync();
                await _context.Entry(a).Reference(aa => aa.OrdenCompra).LoadAsync();
            }

            return activosCreados;
        }
    }
}
