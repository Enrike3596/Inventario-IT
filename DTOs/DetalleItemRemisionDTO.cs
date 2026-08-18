using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DetalleItemRemisionCreateDTO
    {
        [Required(ErrorMessage = "El item de remisión es obligatorio")]
        public int IdItemRemision { get; set; }

        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        public string? Observaciones { get; set; }
    }

    public class DetalleItemRemisionUpdateDTO
    {
        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        public string? Observaciones { get; set; }
    }

    public class DetalleItemRemisionResponseDTO
    {
        public int IdDetalleItemRemision { get; set; }
        public int IdItemRemision { get; set; }
        public string Serial { get; set; } = null!;
        public bool Procesado { get; set; }
        public bool Estado { get; set; }
        public int? IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }

    public class DetalleItemRemisionBatchCreateDTO
    {
        [Required(ErrorMessage = "El item de remisión es obligatorio")]
        public int IdItemRemision { get; set; }

        [Required(ErrorMessage = "Debe ingresar al menos un serial")]
        public List<string> Seriales { get; set; } = new();
    }
}
