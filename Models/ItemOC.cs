using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ItemOC
    {
        [Key]
        public int IdItemOC { get; set; }

        [ForeignKey("OrdenCompra")]
        public int IdOrden { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [Required, MaxLength(100)]
        public string NombreProducto { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Marca { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Modelo { get; set; } = null!;

        [MaxLength(100)]
        public string? Referencia { get; set; }

        public string? Observaciones { get; set; }

        public int CantidadEsperada { get; set; }

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        // Navegación
        public virtual OrdenCompra OrdenCompra { get; set; } = null!;
        public virtual CategoriaActivo Categoria { get; set; } = null!;

        public virtual ICollection<DetalleItemOC> DetallesItem { get; set; } = new List<DetalleItemOC>();
        public virtual ICollection<Activos> Activos { get; set; } = new List<Activos>();
    }
}
