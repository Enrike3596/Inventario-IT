using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class HistorialActivoCreateDTO
    {
        [Required]
        public int IdActivo { get; set; }

        public int? IdSalida { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        public TipoMovimiento TipoMovimiento { get; set; }

        public int? IdUsuarioEntrega { get; set; }
    }

    public class HistorialActivoResponseDTO
    {
        public int IdHistorial { get; set; }
        public int IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Serial { get; set; }
        public int? IdSalida { get; set; }
        public string? CodigoSalida { get; set; }
        public string? EstadoActivoSalida { get; set; }
        public string? Observaciones { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int? IdUsuarioEntrega { get; set; }
        public string? NombreUsuarioEntrega { get; set; }
        public int? IdAsignacion { get; set; }
        public string? NombreUsuarioAsignado { get; set; }
        public string? RegistroSalidaAsignacion { get; set; }
        public string? EstadoAnterior { get; set; }
        public string? EstadoNuevo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
