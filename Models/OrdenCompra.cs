using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class OrdenCompra
    {
        [Key]
        public int IdOrden { get; set; }
        public string NumeroOC { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public decimal Total { get; set; }
        public string Observaciones { get; set; } = null!;
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<ItemOC> ItemsOC { get; set; } = new List<ItemOC>();
    }
}
