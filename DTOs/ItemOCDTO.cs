using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ItemOCCreateDTO
    {
        [Required(ErrorMessage = "La orden de compra es obligatoria")]
        public int IdOrden { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string NombreProducto { get; set; } = null!;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        public string? Referencia { get; set; }

        public string? Observaciones { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad esperada debe ser mayor a 0")]
        public int CantidadEsperada { get; set; } = 1;
    }

    public class ItemOCUpdateDTO
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string NombreProducto { get; set; } = null!;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        public string? Referencia { get; set; }

        public string? Observaciones { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad esperada debe ser mayor a 0")]
        public int CantidadEsperada { get; set; } = 1;
    }

    public class ItemOCResponseDTO
    {
        public int IdItemOC { get; set; }
        public int IdOrden { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string? Referencia { get; set; }
        public string? Observaciones { get; set; }
        public int CantidadEsperada { get; set; }
        public int CantidadIngresada { get; set; }
        public List<DetalleItemOCResponseDTO> DetallesItem { get; set; } = new();
    }
}
