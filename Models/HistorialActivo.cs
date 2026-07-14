using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Models
{
    public class HistorialActivo
    {
        [Key]
        public int IdHistorial { get; set; }

        [ForeignKey("Activo")]
        public int IdActivo { get; set; }

        [ForeignKey("Salida")]
        public int? IdSalida { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;

        [ForeignKey("UsuarioEntrega")]
        public int? IdUsuarioEntrega { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual Activos Activo { get; set; } = null!;
        public virtual Salida? Salida { get; set; }
        public virtual Usuarios? UsuarioEntrega { get; set; }
    }
}
