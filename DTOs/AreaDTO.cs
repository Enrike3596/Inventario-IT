using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class AreaCreateDTO
    {
        [Required(ErrorMessage = "El nombre del área es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del área debe tener entre 2 y 100 caracteres.")]
        public string NombreArea { get; set; } = null!;
    }

    public class AreaUpdateDTO
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del área debe tener entre 2 y 100 caracteres.")]
        public string? NombreArea { get; set; }

        public bool? Estado { get; set; }

        public string? MotivoEdicion { get; set; }
    }

    public class AreaResponseDTO
    {
        public int IdArea { get; set; }
        public string NombreArea { get; set; } = null!;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
