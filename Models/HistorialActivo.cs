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
        public int IdSalida { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [ForeignKey("UsuarioEntrega")]
        public int IdUsuarioEntrega { get; set; }

        public virtual Activos Activo { get; set; } = null!;
        public virtual Salida Salida { get; set; } = null!;
        public virtual Usuarios UsuarioEntrega { get; set; } = null!;
    }
}
