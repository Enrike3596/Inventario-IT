using DTOs;
using Models;
using Repositories;
using Services.FileStorage;

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
        private readonly IFileStorageService _fileStorage;

        public RemisionService(IRemisionRepository repo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _fileStorage = fileStorage;
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
            ValidarDocumento(dto.RutaDocumento, dto.NombreDocumento);

            var remision = new Remision
            {
                NumeroRemision = dto.NumeroRemision,
                Proveedor = dto.Proveedor,
                RutaDocumento = dto.RutaDocumento,
                NombreDocumento = dto.NombreDocumento
            };

            var creada = await _repo.CrearAsync(remision);
            return MapToDTO(creada);
        }

        public async Task<RemisionResponseDTO?> ActualizarAsync(int id, RemisionUpdateDTO dto)
        {
            ValidarDocumento(dto.RutaDocumento, dto.NombreDocumento);

            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var remision = await _repo.ObtenerPorIdAsync(id);
            var rutaDocumento = remision?.RutaDocumento;

            var eliminada = await _repo.EliminarAsync(id);
            if (eliminada && !string.IsNullOrWhiteSpace(rutaDocumento))
            {
                var (contenedor, nombreArchivo) = DescomponerRuta(rutaDocumento);
                await _fileStorage.DeleteAsync(contenedor, nombreArchivo);
            }
            return eliminada;
        }

        private static void ValidarDocumento(string? ruta, string? nombre)
        {
            if (string.IsNullOrWhiteSpace(ruta) || string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El documento PDF de la remisión es obligatorio.");
        }

        private static (string Contenedor, string NombreArchivo) DescomponerRuta(string ruta)
        {
            var limpia = ruta.Replace('\\', '/');
            var idx = limpia.IndexOf('/');
            if (idx <= 0 || idx == limpia.Length - 1)
                return ("", limpia);
            return (limpia[..idx], limpia[(idx + 1)..]);
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
                RutaDocumento = r.RutaDocumento,
                NombreDocumento = r.NombreDocumento,
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
                RutaDocumento = r.RutaDocumento,
                NombreDocumento = r.NombreDocumento,
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