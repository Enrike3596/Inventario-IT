using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; } = null!;
    }

    public class TokenResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expira { get; set; }
        public UsuarioResponseDTO Usuario { get; set; } = null!;
    }

    public class SolicitarRestablecimientoDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = string.Empty;
    }

    public class RestablecerContrasenaDTO
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}
