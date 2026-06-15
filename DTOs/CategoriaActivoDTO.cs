using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class CategoriaActivoCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;
    }

    public class CategoriaActivoUpdateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;
    }

    public class CategoriaActivoResponseDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public EstadoGenerico Estado { get; set; }
    }
}
