using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface IRemisionService
    {
        Task<List<RemisionResponseDTO>> ObtenerTodosAsync();
        Task<RemisionResponseDTO?> ObtenerPorIdAsync(int id);
        Task<RemisionDetailDTO?> ObtenerDetalleAsync(int id);
        Task<RemisionResponseDTO> CrearAsync(RemisionCreateDTO dto);
        Task<RemisionResponseDTO?> ActualizarAsync(int id, RemisionUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<List<ActivoResponseDTO>> ConfirmarIngresoAsync(int idRemision);
    }

    public class RemisionService : IRemisionService
    {
        private readonly IRemisionRepository _repo;

        public RemisionService(IRemisionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RemisionResponseDTO>> ObtenerTodosAsync()
        {
            var remisiones = await _repo.ObtenerTodosAsync();
            return remisiones.Select(MapToDTO).ToList();
        }

        public async Task<RemisionResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var remision = await _repo.ObtenerPorIdAsync(id);
            return remision == null ? null : MapToDTO(remision);
        }

        public async Task<RemisionDetailDTO?> ObtenerDetalleAsync(int id)
        {
            var remision = await _repo.ObtenerConItemsAsync(id);
            return remision == null ? null : MapToDetailDTO(remision);
        }

        public async Task<RemisionResponseDTO> CrearAsync(RemisionCreateDTO dto)
        {
            var remision = new Remision
            {
                NumeroRemision = dto.NumeroRemision,
                Proveedor = dto.Proveedor
            };

            var creada = await _repo.CrearAsync(remision);
            return MapToDTO(creada);
        }

        public async Task<RemisionResponseDTO?> ActualizarAsync(int id, RemisionUpdateDTO dto)
        {
            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        public async Task<List<ActivoResponseDTO>> ConfirmarIngresoAsync(int idRemision)
        {
            var activos = await _repo.ConfirmarIngresoAsync(idRemision);
            return activos.Select(a => new ActivoResponseDTO
            {
                IdActivo = a.IdActivo,
                IdCategoria = a.IdCategoria,
                NombreCategoria = a.Categoria?.Nombre,
                IdRemision = a.IdRemision,
                NumeroRemision = a.Remision?.NumeroRemision,
                IdItemRemision = a.IdItemRemision,
                IdDetalleItemRemision = a.IdDetalleItemRemision,
                CodigoActivo = a.CodigoActivo,
                Serial = a.Serial,
                Marca = a.Marca,
                Modelo = a.Modelo,
                EstadoActivo = a.EstadoActivo,
                FechaAdquisicion = a.FechaAdquisicion,
                FechaBaja = a.FechaBaja,
                Observaciones = a.Observaciones,
                FechaCreacion = a.FechaCreacion,
                FechaModificacion = a.FechaModificacion,
                CreadoPor = a.CreadoPor,
                ModificadoPor = a.ModificadoPor
            }).ToList();
        }

        private static RemisionResponseDTO MapToDTO(Remision r)
        {
            return new RemisionResponseDTO
            {
                IdRemision = r.IdRemision,
                NumeroRemision = r.NumeroRemision,
                Proveedor = r.Proveedor,
                FechaCompra = r.FechaCompra,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaModificacion = r.FechaModificacion,
                CreadoPor = r.CreadoPor,
                ModificadoPor = r.ModificadoPor,
                ItemsRemision = r.ItemsRemision?.Select(i => new ItemRemisionResponseDTO
                {
                    IdItemRemision = i.IdItemRemision,
                    IdRemision = i.IdRemision,
                    IdCategoria = i.IdCategoria,
                    NombreCategoria = i.Categoria?.Nombre,
                    Marca = i.Marca,
                    Modelo = i.Modelo,
                    CantidadEsperada = i.CantidadEsperada,
                    CantidadIngresada = i.DetallesItem?.Count(d => d.Procesado) ?? 0,
                    Estado = i.Estado,
                    FechaCreacion = i.FechaCreacion,
                    FechaModificacion = i.FechaModificacion,
                    CreadoPor = i.CreadoPor,
                    ModificadoPor = i.ModificadoPor,
                    DetallesItem = i.DetallesItem?.Select(d => new DetalleItemRemisionResponseDTO
                    {
                        IdDetalleItemRemision = d.IdDetalleItemRemision,
                        IdItemRemision = d.IdItemRemision,
                        Serial = d.Serial,
                        Procesado = d.Procesado,
                        Estado = d.Estado,
                        IdActivo = d.IdActivo,
                        CodigoActivo = d.Activo?.CodigoActivo,
                        Observaciones = d.Observaciones,
                        FechaCreacion = d.FechaCreacion,
                        FechaModificacion = d.FechaModificacion,
                        CreadoPor = d.CreadoPor,
                        ModificadoPor = d.ModificadoPor
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };
        }

        private static RemisionDetailDTO MapToDetailDTO(Remision r)
        {
            return new RemisionDetailDTO
            {
                IdRemision = r.IdRemision,
                NumeroRemision = r.NumeroRemision,
                Proveedor = r.Proveedor,
                FechaCompra = r.FechaCompra,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaModificacion = r.FechaModificacion,
                CreadoPor = r.CreadoPor,
                ModificadoPor = r.ModificadoPor,
                ItemsRemision = r.ItemsRemision?.Select(i => new ItemRemisionResponseDTO
                {
                    IdItemRemision = i.IdItemRemision,
                    IdRemision = i.IdRemision,
                    IdCategoria = i.IdCategoria,
                    NombreCategoria = i.Categoria?.Nombre,
                    Marca = i.Marca,
                    Modelo = i.Modelo,
                    CantidadEsperada = i.CantidadEsperada,
                    CantidadIngresada = i.DetallesItem?.Count(d => d.Procesado) ?? 0,
                    Estado = i.Estado,
                    FechaCreacion = i.FechaCreacion,
                    FechaModificacion = i.FechaModificacion,
                    CreadoPor = i.CreadoPor,
                    ModificadoPor = i.ModificadoPor,
                    DetallesItem = i.DetallesItem?.Select(d => new DetalleItemRemisionResponseDTO
                    {
                        IdDetalleItemRemision = d.IdDetalleItemRemision,
                        IdItemRemision = d.IdItemRemision,
                        Serial = d.Serial,
                        Procesado = d.Procesado,
                        Estado = d.Estado,
                        IdActivo = d.IdActivo,
                        CodigoActivo = d.Activo?.CodigoActivo,
                        Observaciones = d.Observaciones,
                        FechaCreacion = d.FechaCreacion,
                        FechaModificacion = d.FechaModificacion,
                        CreadoPor = d.CreadoPor,
                        ModificadoPor = d.ModificadoPor
                    }).ToList() ?? new()
                }).ToList() ?? new()
            };
        }
    }
}