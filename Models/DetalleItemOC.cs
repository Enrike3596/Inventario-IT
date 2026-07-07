using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class DetalleItemOC
    {
        [Key]
        public int IdDetalleItemOC { get; set; }

        [ForeignKey("ItemOC")]
        public int IdItemOC { get; set; }

        [Required, MaxLength(100)]
        public string Serial { get; set; } = null!;

        public bool Procesado { get; set; } = false;

        [ForeignKey("Activo")]
        public int? IdActivo { get; set; }

        public string? Observaciones { get; set; }

        // Navegación
        public virtual ItemOC ItemOC { get; set; } = null!;
        public virtual Activos? Activo { get; set; }
    }
}
