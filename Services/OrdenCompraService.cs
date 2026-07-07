using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompraResponseDTO>> ObtenerTodosAsync();
        Task<OrdenCompraResponseDTO?> ObtenerPorIdAsync(int id);
        Task<OrdenCompraDetailDTO?> ObtenerDetalleAsync(int id);
        Task<OrdenCompraResponseDTO> CrearAsync(OrdenCompraCreateDTO dto);
        Task<OrdenCompraResponseDTO?> ActualizarAsync(int id, OrdenCompraUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<List<ActivoResponseDTO>> ConfirmarIngresoAsync(int idOrden);
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

        public async Task<OrdenCompraDetailDTO?> ObtenerDetalleAsync(int id)
        {
            var orden = await _repo.ObtenerConItemsAsync(id);
            return orden == null ? null : MapToDetailDTO(orden);
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

        public async Task<List<ActivoResponseDTO>> ConfirmarIngresoAsync(int idOrden)
        {
            var activos = await _repo.ConfirmarIngresoAsync(idOrden);
            return activos.Select(a => new ActivoResponseDTO
            {
                IdActivo = a.IdActivo,
                IdCategoria = a.IdCategoria,
                NombreCategoria = a.Categoria?.Nombre,
                IdOrden = a.IdOrden,
                NumeroOC = a.OrdenCompra?.NumeroOC,
                IdItemOC = a.IdItemOC,
                IdDetalleItemOC = a.IdDetalleItemOC,
                CodigoActivo = a.CodigoActivo,
                Serial = a.Serial,
                Marca = a.Marca,
                Modelo = a.Modelo,
                Referencia = a.Referencia,
                EstadoActivo = a.EstadoActivo,
                FechaAdquisicion = a.FechaAdquisicion,
                FechaBaja = a.FechaBaja,
                Observaciones = a.Observaciones
            }).ToList();
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
                FechaCompra = o.FechaCompra,
                ItemsOC = o.ItemsOC?.Select(i => new ItemOCResponseDTO
                {
                    IdItemOC = i.IdItemOC,
                    IdOrden = i.IdOrden,
                    IdCategoria = i.IdCategoria,
                    NombreCategoria = i.Categoria?.Nombre,
                    NombreProducto = i.NombreProducto,
                    Marca = i.Marca,
                    Modelo = i.Modelo,
                    Referencia = i.Referencia,
                    Observaciones = i.Observaciones,
                    CantidadEsperada = i.CantidadEsperada,
                    CantidadIngresada = i.DetallesItem?.Count(d => d.Procesado) ?? 0,
                    DetallesItem = i.DetallesItem?.Select(d => new DetalleItemOCResponseDTO
                    {
                        IdDetalleItemOC = d.IdDetalleItemOC,
                        IdItemOC = d.IdItemOC,
                        Serial = d.Serial,
                        Procesado = d.Procesado,
                        IdActivo = d.IdActivo,
                        CodigoActivo = d.Activo?.CodigoActivo,
                        Observaciones = d.Observaciones
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };
        }

        private static OrdenCompraDetailDTO MapToDetailDTO(OrdenCompra o)
        {
            return new OrdenCompraDetailDTO
            {
                IdOrden = o.IdOrden,
                NumeroOC = o.NumeroOC,
                Proveedor = o.Proveedor,
                Total = o.Total,
                Observaciones = o.Observaciones,
                FechaCompra = o.FechaCompra,
                ItemsOC = o.ItemsOC?.Select(i => new ItemOCResponseDTO
                {
                    IdItemOC = i.IdItemOC,
                    IdOrden = i.IdOrden,
                    IdCategoria = i.IdCategoria,
                    NombreCategoria = i.Categoria?.Nombre,
                    NombreProducto = i.NombreProducto,
                    Marca = i.Marca,
                    Modelo = i.Modelo,
                    Referencia = i.Referencia,
                    Observaciones = i.Observaciones,
                    CantidadEsperada = i.CantidadEsperada,
                    CantidadIngresada = i.DetallesItem?.Count(d => d.Procesado) ?? 0,
                    DetallesItem = i.DetallesItem?.Select(d => new DetalleItemOCResponseDTO
                    {
                        IdDetalleItemOC = d.IdDetalleItemOC,
                        IdItemOC = d.IdItemOC,
                        Serial = d.Serial,
                        Procesado = d.Procesado,
                        IdActivo = d.IdActivo,
                        CodigoActivo = d.Activo?.CodigoActivo,
                        Observaciones = d.Observaciones
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };
        }
    }
}
