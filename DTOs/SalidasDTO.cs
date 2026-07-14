using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class SalidaCreateDTO
    {
        public int? IdUsuarioDestino { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un activo")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un activo")]
        public List<SalidaActivoDTO> Activos { get; set; } = new();
    }

    public class SalidaActivoDTO
    {
        [Required]
        public int IdActivo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }

    public class SalidaUpdateDTO
    {
        public string? Observaciones { get; set; }

        public string? MotivoEdicion { get; set; }
    }

    public class SalidaResponseDTO
    {
        public int IdSalida { get; set; }
        public string CodigoUnico { get; set; } = null!;
        public int? IdUsuarioDestino { get; set; }
        public string? NombreUsuarioDestino { get; set; }
        public DateTime FechaSalida { get; set; }
        public string? Observaciones { get; set; }
        public List<DetalleSalidaResponseDTO> Detalles { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
