using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class CanalCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;
    }

    public class CanalUpdateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        public string? MotivoEdicion { get; set; }
    }

    public class CanalResponseDTO
    {
        public int IdCanal { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
