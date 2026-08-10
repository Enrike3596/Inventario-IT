using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public interface IActaFirmaRepository
    {
        Task<ActaFirma?> ObtenerPorDestinoAsync(int idDestino, string tipoDestino);
        Task<ActaFirma?> ObtenerPorTokenAsync(string token);
        Task<ActaFirma> CrearAsync(ActaFirma acta);
        Task<ActaFirma?> ActualizarAsync(ActaFirma acta);
        Task EliminarAsync(ActaFirma acta);
    }

    public class ActaFirmaRepository : IActaFirmaRepository
    {
        private readonly AppDbContext _context;

        public ActaFirmaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActaFirma?> ObtenerPorDestinoAsync(int idDestino, string tipoDestino)
        {
            return await _context.Set<ActaFirma>()
                .Where(a => a.IdDestino == idDestino && a.TipoDestino == tipoDestino && a.Activa)
                .OrderByDescending(a => a.FechaGeneracion)
                .FirstOrDefaultAsync();
        }

        public async Task<ActaFirma?> ObtenerPorTokenAsync(string token)
        {
            return await _context.Set<ActaFirma>()
                .Where(a => a.Token == token && a.Activa)
                .FirstOrDefaultAsync();
        }

        public async Task<ActaFirma> CrearAsync(ActaFirma acta)
        {
            _context.Set<ActaFirma>().Add(acta);
            await _context.SaveChangesAsync();
            return acta;
        }

        public async Task<ActaFirma?> ActualizarAsync(ActaFirma acta)
        {
            _context.Set<ActaFirma>().Update(acta);
            await _context.SaveChangesAsync();
            return acta;
        }

        public async Task EliminarAsync(ActaFirma acta)
        {
            acta.Activa = false;
            await _context.SaveChangesAsync();
        }
    }
}
