using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class OrdenCompraCreateDTO
    {
        [Required(ErrorMessage = "El número de OC es obligatorio")]
        public string NumeroOC { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        public string Observaciones { get; set; } = null!;
    }

    public class OrdenCompraUpdateDTO
    {
        [Required(ErrorMessage = "El número de OC es obligatorio")]
        public string NumeroOC { get; set; } = null!;

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public string Proveedor { get; set; } = null!;

        [Required(ErrorMessage = "El total es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        public string Observaciones { get; set; } = null!;
    }

    public class OrdenCompraResponseDTO
    {
        public int IdOrden { get; set; }
        public string NumeroOC { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public decimal Total { get; set; }
        public string Observaciones { get; set; } = null!;
        public DateTime FechaCompra { get; set; }
    }
}
