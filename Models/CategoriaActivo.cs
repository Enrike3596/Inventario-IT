using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class CategoriaActivo
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<Activos> Activos { get; set; } = new List<Activos>();
        public virtual ICollection<ItemOC> ItemsOC { get; set; } = new List<ItemOC>();
    }
}
