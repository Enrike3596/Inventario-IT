using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class ParqueaderoCreateDTO
    {
        [Required(ErrorMessage = "La sede es obligatoria")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; } = null!;
    }

    public class ParqueaderoUpdateDTO
    {
        [Required(ErrorMessage = "La sede es obligatoria")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; } = null!;

        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;
    }

    public class ParqueaderoResponseDTO
    {
        public int IdParqueadero { get; set; }
        public int IdSede { get; set; }
        public string? NombreSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public EstadoGenerico Estado { get; set; }
    }
}
