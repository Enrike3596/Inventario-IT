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

        public virtual ICollection<Activos> Activos { get; set; } = new List<Activos>();
    }
}
