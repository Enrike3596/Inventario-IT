using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class ParqueaderoCreateDTO
    {
        [Required(ErrorMessage = "El DA es obligatorio")]
        [MaxLength(50, ErrorMessage = "El DA no puede exceder 50 caracteres")]
        public string DA { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; } = null!;
    }

    public class ParqueaderoUpdateDTO
    {
        [Required(ErrorMessage = "El DA es obligatorio")]
        [MaxLength(50, ErrorMessage = "El DA no puede exceder 50 caracteres")]
        public string DA { get; set; } = null!;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; } = null!;

        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;

        public string? MotivoEdicion { get; set; }
    }

    public class ParqueaderoResponseDTO
    {
        public int IdParqueadero { get; set; }
        public string DA { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public EstadoGenerico Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
