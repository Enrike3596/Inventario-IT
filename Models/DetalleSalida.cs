using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class DetalleSalida
    {
        [Key]
        public int IdDetalleSalida { get; set; }

        [ForeignKey("Salida")]
        public int IdSalida { get; set; }

        [ForeignKey("Activo")]
        public int IdActivo { get; set; }

        public int Cantidad { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual Salida Salida { get; set; } = null!;
        public virtual Activos Activo { get; set; } = null!;
    }
}
