using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class DetalleItemRemision
    {
        [Key]
        public int IdDetalleItemRemision { get; set; }

        [ForeignKey("ItemRemision")]
        public int IdItemRemision { get; set; }

        [Required, MaxLength(100)]
        public string Serial { get; set; } = null!;

        public bool Procesado { get; set; } = false;

        public bool Estado { get; set; } = true;

        [ForeignKey("Activo")]
        public int? IdActivo { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        // Navegación
        public virtual ItemRemision ItemRemision { get; set; } = null!;
        public virtual Activos? Activo { get; set; }
    }
}
