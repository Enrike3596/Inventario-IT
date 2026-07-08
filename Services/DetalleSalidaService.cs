using DTOs;
using Repositories;

namespace Services
{
    public interface IDetalleSalidaService
    {
        Task<List<DetalleSalidaResponseDTO>> ObtenerPorSalidaAsync(int idSalida);
        Task<DetalleSalidaResponseDTO?> ObtenerPorIdAsync(int id);
    }

    public class DetalleSalidaService : IDetalleSalidaService
    {
        private readonly IDetalleSalidaRepository _repo;

        public DetalleSalidaService(IDetalleSalidaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DetalleSalidaResponseDTO>> ObtenerPorSalidaAsync(int idSalida)
        {
            var detalles = await _repo.ObtenerPorSalidaAsync(idSalida);
            return detalles.Select(d => new DetalleSalidaResponseDTO
            {
                IdDetalleSalida = d.IdDetalleSalida,
                IdSalida = d.IdSalida,
                IdActivo = d.IdActivo,
                CodigoActivo = d.Activo?.CodigoActivo,
                Serial = d.Activo?.Serial,
                Marca = d.Activo?.Marca,
                Modelo = d.Activo?.Modelo,
                Cantidad = d.Cantidad,
                FechaCreacion = d.FechaCreacion,
                FechaModificacion = d.FechaModificacion,
                CreadoPor = d.CreadoPor,
                ModificadoPor = d.ModificadoPor
            }).ToList();
        }

        public async Task<DetalleSalidaResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var detalle = await _repo.ObtenerPorIdAsync(id);
            if (detalle == null) return null;

            return new DetalleSalidaResponseDTO
            {
                IdDetalleSalida = detalle.IdDetalleSalida,
                IdSalida = detalle.IdSalida,
                IdActivo = detalle.IdActivo,
                CodigoActivo = detalle.Activo?.CodigoActivo,
                Serial = detalle.Activo?.Serial,
                Marca = detalle.Activo?.Marca,
                Modelo = detalle.Activo?.Modelo,
                Cantidad = detalle.Cantidad,
                FechaCreacion = detalle.FechaCreacion,
                FechaModificacion = detalle.FechaModificacion,
                CreadoPor = detalle.CreadoPor,
                ModificadoPor = detalle.ModificadoPor
            };
        }
    }
}
