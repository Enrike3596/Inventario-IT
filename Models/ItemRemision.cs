using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ItemRemision
    {
        [Key]
        public int IdItemRemision { get; set; }

        [ForeignKey("Remision")]
        public int IdRemision { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [Required, MaxLength(100)]
        public string Marca { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Modelo { get; set; } = null!;

        public int CantidadEsperada { get; set; }

        public bool Estado { get; set; } = true;

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        // Navegación
        public virtual Remision Remision { get; set; } = null!;
        public virtual CategoriaActivo Categoria { get; set; } = null!;

        public virtual ICollection<DetalleItemRemision> DetallesItem { get; set; } = new List<DetalleItemRemision>();
        public virtual ICollection<Activos> Activos { get; set; } = new List<Activos>();
    }
}
