using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ItemRemisionCreateDTO
    {
        [Required(ErrorMessage = "La remisión es obligatoria")]
        public int IdRemision { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad esperada debe ser mayor a 0")]
        public int CantidadEsperada { get; set; } = 1;
    }

    public class ItemRemisionUpdateDTO
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad esperada debe ser mayor a 0")]
        public int CantidadEsperada { get; set; } = 1;

        public string? MotivoEdicion { get; set; }
    }

    public class ItemRemisionResponseDTO
    {
        public int IdItemRemision { get; set; }
        public int IdRemision { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public int CantidadEsperada { get; set; }
        public int CantidadIngresada { get; set; }
        public bool Estado { get; set; }
        public List<DetalleItemRemisionResponseDTO> DetallesItem { get; set; } = new();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
