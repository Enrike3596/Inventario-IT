using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Remision
    {
        [Key]
        public int IdRemision { get; set; }
        public string NumeroRemision { get; set; } = null!;
        public string Proveedor { get; set; } = null!;
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
        public bool Estado { get; set; } = true;
        public string? RutaDocumento { get; set; }
        public string? NombreDocumento { get; set; }
        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<ItemRemision> ItemsRemision { get; set; } = new List<ItemRemision>();
    }
}
