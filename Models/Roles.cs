using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class Roles
    {
        [Key]
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
    }
}
