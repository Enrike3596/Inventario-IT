using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class UsuarioCreateDTO
    {
        [Required(ErrorMessage = "El rol es obligatorio")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "La sede es obligatoria")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Contraseña { get; set; } = null!;
    }

    public class UsuarioUpdateDTO
    {
        [Required(ErrorMessage = "El rol es obligatorio")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "La sede es obligatoria")]
        public int IdSede { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; } = null!;

        public EstadoUsuario EstadoUsuario { get; set; } = EstadoUsuario.Activo;

        public string? MotivoEdicion { get; set; }
    }

    public class UsuarioResponseDTO
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }
        public int IdSede { get; set; }
        public string? NombreSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Cargo { get; set; } = null!;
        public EstadoUsuario EstadoUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
