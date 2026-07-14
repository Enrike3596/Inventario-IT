using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class AsignacionUsuarioCreateDTO
    {
        [Required(ErrorMessage = "El activo es obligatorio")]
        public int IdActivo { get; set; }

        [Required(ErrorMessage = "El usuario destino es obligatorio")]
        public int IdUsuarioDestino { get; set; }

        public int? IdParqueadero { get; set; }

        [Required(ErrorMessage = "El canal es obligatorio")]
        public int IdCanal { get; set; }

        [Required(ErrorMessage = "El usuario que entrega es obligatorio")]
        public int IdUsuarioEntrega { get; set; }

        [Required(ErrorMessage = "El registro de salida es obligatorio")]
        public string RegistroSalida { get; set; } = null!;

        public string? NumeroTicket { get; set; }
    }

    public class AsignacionUsuarioUpdateDTO
    {
        public EstadoAsignacion EstadoAsignacion { get; set; } = EstadoAsignacion.Activa;

        public string? MotivoEdicion { get; set; }
    }

    public class AsignacionUsuarioResponseDTO
    {
        public int IdAsignacion { get; set; }
        public int IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Serial { get; set; }
        public int IdUsuarioDestino { get; set; }
        public string? NombreUsuarioDestino { get; set; }
        public int? IdParqueadero { get; set; }
        public string? NombreParqueadero { get; set; }
        public int IdCanal { get; set; }
        public string? NombreCanal { get; set; }
        public int IdUsuarioEntrega { get; set; }
        public string? NombreUsuarioEntrega { get; set; }
        public string RegistroSalida { get; set; } = null!;
        public string? NumeroTicket { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public EstadoAsignacion EstadoAsignacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
