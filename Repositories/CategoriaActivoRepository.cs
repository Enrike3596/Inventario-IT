using Data;
using DTOs;
using Enums;
using Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repositories
{
    public interface ICategoriaActivoRepository
    {
        Task<List<CategoriaActivo>> ObtenerTodosAsync();
        Task<CategoriaActivo?> ObtenerPorIdAsync(int id);
        Task<CategoriaActivo> CrearAsync(CategoriaActivo categoria);
        Task<CategoriaActivo?> ActualizarAsync(int id, CategoriaActivoUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class CategoriaActivoRepository : ICategoriaActivoRepository
    {
        private readonly AppDbContext _context;

        public CategoriaActivoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaActivo>> ObtenerTodosAsync()
        {
            return await _context.CategoriasActivo
                .Where(c => c.Estado == EstadoGenerico.Activo)
                .ToListAsync();
        }

        public async Task<CategoriaActivo?> ObtenerPorIdAsync(int id)
        {
            return await _context.CategoriasActivo.FindAsync(id);
        }

        public async Task<CategoriaActivo> CrearAsync(CategoriaActivo categoria)
        {
            categoria.Nombre = (categoria.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(categoria));

            _context.CategoriasActivo.Add(categoria);

            try
            {
                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg
                                               && pg.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await FixCategoriaIdSequenceAsync();
                _context.Entry(categoria).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return categoria;
            }
        }

        private async Task FixCategoriaIdSequenceAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(
                "SELECT setval(pg_get_serial_sequence('\"CategoriasActivo\"', 'IdCategoria'), (SELECT COALESCE(MAX(\"IdCategoria\"), 0) FROM \"CategoriasActivo\"));");
        }

        public async Task<CategoriaActivo?> ActualizarAsync(int id, CategoriaActivoUpdateDTO dto)
        {
            var categoria = await _context.CategoriasActivo.FindAsync(id);
            if (categoria == null) return null;

            var nombre = (dto.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede ser vacío.", nameof(dto.Nombre));
            categoria.Nombre = nombre;

            categoria.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await _context.CategoriasActivo.FindAsync(id);
            if (categoria == null) return false;

            categoria.Estado = EstadoGenerico.Inactivo;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
