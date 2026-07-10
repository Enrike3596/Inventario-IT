using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class SedeCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; } = null!;
    }

    public class SedeUpdateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; } = null!;

        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;

        public string? MotivoEdicion { get; set; }
    }

    public class SedeResponseDTO
    {
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public EstadoGenerico Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
