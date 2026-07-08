using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DetalleSalidaCreateDTO
    {
        [Required]
        public int IdActivo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }

    public class DetalleSalidaResponseDTO
    {
        public int IdDetalleSalida { get; set; }
        public int IdSalida { get; set; }
        public int IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Serial { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
