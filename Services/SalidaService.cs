using DTOs;
using Models;
using Repositories;

namespace Services
{
    public interface ISalidaService
    {
        Task<List<SalidaResponseDTO>> ObtenerTodosAsync();
        Task<SalidaResponseDTO?> ObtenerPorIdAsync(int id);
        Task<SalidaResponseDTO> CrearAsync(SalidaCreateDTO dto);
        Task<SalidaResponseDTO?> ActualizarAsync(int id, SalidaUpdateDTO dto);
        Task<bool> EliminarAsync(int id);
    }

    public class SalidaService : ISalidaService
    {
        private readonly ISalidaRepository _repo;

        public SalidaService(ISalidaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SalidaResponseDTO>> ObtenerTodosAsync()
        {
            var salidas = await _repo.ObtenerTodosAsync();
            return salidas.Select(MapToDTO).ToList();
        }

        public async Task<SalidaResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var salida = await _repo.ObtenerPorIdAsync(id);
            return salida == null ? null : MapToDTO(salida);
        }

        public async Task<SalidaResponseDTO> CrearAsync(SalidaCreateDTO dto)
        {
            var salida = new Salida
            {
                IdCanal = dto.IdCanal,
                NumeroTicket = dto.NumeroTicket,
                IdUsuarioDestino = dto.IdUsuarioDestino,
                IdParqueaderoDestino = dto.IdParqueaderoDestino,
                IdUsuarioEntrega = dto.IdUsuarioEntrega,
                RegistroSalida = dto.RegistroSalida,
                Observaciones = dto.Observaciones
            };

            var detalles = dto.Activos.Select(a => new DetalleSalida
            {
                IdActivo = a.IdActivo,
                Cantidad = a.Cantidad
            }).ToList();

            var creada = await _repo.CrearAsync(salida, detalles);
            return MapToDTO(creada);
        }

        public async Task<SalidaResponseDTO?> ActualizarAsync(int id, SalidaUpdateDTO dto)
        {
            var actualizada = await _repo.ActualizarAsync(id, dto);
            return actualizada == null ? null : MapToDTO(actualizada);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        private static SalidaResponseDTO MapToDTO(Salida s)
        {
            return new SalidaResponseDTO
            {
                IdSalida = s.IdSalida,
                IdCanal = s.IdCanal,
                NombreCanal = s.CanalSolicitud?.Nombre,
                CodigoUnico = s.CodigoUnico,
                NumeroTicket = s.NumeroTicket,
                IdUsuarioDestino = s.IdUsuarioDestino,
                NombreUsuarioDestino = s.UsuarioDestino?.Nombre,
                IdParqueaderoDestino = s.IdParqueaderoDestino,
                NombreParqueaderoDestino = s.ParqueaderoDestino?.Nombre,
                IdUsuarioEntrega = s.IdUsuarioEntrega,
                NombreUsuarioEntrega = s.UsuarioEntrega?.Nombre,
                FechaSalida = s.FechaSalida,
                RegistroSalida = s.RegistroSalida,
                Observaciones = s.Observaciones,
                FechaCreacion = s.FechaCreacion,
                FechaModificacion = s.FechaModificacion,
                CreadoPor = s.CreadoPor,
                ModificadoPor = s.ModificadoPor,
                Detalles = s.DetallesSalida.Select(d => new DetalleSalidaResponseDTO
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
                }).ToList()
            };
        }
    }
}
