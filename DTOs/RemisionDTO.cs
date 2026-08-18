using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class RemisionCreateDTO
    {
        [Required(ErrorMessage = "El número de remisión es obligatorio")]
        public string NumeroRemision { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Required(ErrorMessage = "El documento PDF de la remisión es obligatorio")]
        public string RutaDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del documento de la remisión es obligatorio")]
        public string NombreDocumento { get; set; } = null!;
    }

    public class RemisionUpdateDTO
    {
        [Required(ErrorMessage = "El número de remisión es obligatorio")]
        public string NumeroRemision { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Required(ErrorMessage = "El documento PDF de la remisión es obligatorio")]
        public string RutaDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del documento de la remisión es obligatorio")]
        public string NombreDocumento { get; set; } = null!;

        public string? MotivoEdicion { get; set; }
    }

    public class RemisionResponseDTO
    {
        public int IdRemision { get; set; }
        public string NumeroRemision { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public DateTime FechaCompra { get; set; }
        public bool Estado { get; set; }
        public string? RutaDocumento { get; set; }
        public string? NombreDocumento { get; set; }
        public List<ItemRemisionResponseDTO> ItemsRemision { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }

    public class RemisionDetailDTO
    {
        public int IdRemision { get; set; }
        public string NumeroRemision { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public DateTime FechaCompra { get; set; }
        public bool Estado { get; set; }
        public string? RutaDocumento { get; set; }
        public string? NombreDocumento { get; set; }
        public List<ItemRemisionResponseDTO> ItemsRemision { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }

    public class RemisionDocumentoDTO
    {
        public string RutaDocumento { get; set; } = null!;
        public string NombreDocumento { get; set; } = null!;
    }
}
