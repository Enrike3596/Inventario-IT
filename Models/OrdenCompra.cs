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
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        public virtual ICollection<Activos> Activos { get; set; } = new List<Activos>();
    }
}
