using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class OrdenCompraCreateDTO
    {
        [Required(ErrorMessage = "El número de OC es obligatorio")]
        public string NumeroOC { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }

        public string? Observaciones { get; set; }
    }

    public class OrdenCompraUpdateDTO
    {
        [Required(ErrorMessage = "El número de OC es obligatorio")]
        public string NumeroOC { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal Total { get; set; }

        public string? Observaciones { get; set; }

        public string? MotivoEdicion { get; set; }
    }

    public class OrdenCompraResponseDTO
    {
        public int IdOrden { get; set; }
        public string NumeroOC { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<ItemOCResponseDTO> ItemsOC { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }

    public class OrdenCompraDetailDTO
    {
        public int IdOrden { get; set; }
        public string NumeroOC { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public decimal Total { get; set; }
        public string Observaciones { get; set; } = null!;
        public DateTime FechaCompra { get; set; }
        public List<ItemOCResponseDTO> ItemsOC { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
