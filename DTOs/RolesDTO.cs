using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class RolCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; } = null!;
    }

    public class RolUpdateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; } = null!;

        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;

        public string? MotivoEdicion { get; set; }
    }

    public class RolResponseDTO
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public EstadoGenerico Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
