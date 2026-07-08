using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Canal
    {
        [Key]
        public int IdCanal { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<Salida> Salidas { get; set; } = new List<Salida>();
    }
}
