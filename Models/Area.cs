using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }
        public string NombreArea { get; set; } = null!;
        public bool Estado { get; set; } = true;
        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
    }
}
