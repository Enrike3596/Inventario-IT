using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompraResponseDTO>> ObtenerTodosAsync();
        Task<OrdenCompraResponseDTO?> ObtenerPorIdAsync(int id);
        Task<OrdenCompraResponseDTO> CrearAsync(OrdenCompraCreateDTO dto);
        Task<OrdenCompraResponseDTO?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IOrdenCompraRepository _repo;

        public OrdenCompraService(IOrdenCompraRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrdenCompraResponseDTO>> ObtenerTodosAsync()
        {
            var ordenes = await _repo.ObtenerTodosAsync();
            return ordenes.Select(MapToDTO).ToList();
        }

        public async Task<OrdenCompraResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var orden = await _repo.ObtenerPorIdAsync(id);
            return orden == null ? null : MapToDTO(orden);
        }

        public async Task<OrdenCompraResponseDTO> CrearAsync(OrdenCompraCreateDTO dto)
        {
            var orden = new OrdenCompra
            {
                NumeroOC = dto.NumeroOC,
                Proveedor = dto.Proveedor,
                Total = dto.Total,
                Observaciones = dto.Observaciones
            };

            var creada = await _repo.CrearAsync(orden);
            return MapToDTO(creada);
        }

        public async Task<OrdenCompraResponseDTO?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto)
        {
            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static OrdenCompraResponseDTO MapToDTO(OrdenCompra o)
        {
            return new OrdenCompraResponseDTO
            {
                IdOrden = o.IdOrden,
                NumeroOC = o.NumeroOC,
                Proveedor = o.Proveedor,
                Total = o.Total,
                Observaciones = o.Observaciones,
                FechaCompra = o.FechaCompra
            };
        }
    }
}
