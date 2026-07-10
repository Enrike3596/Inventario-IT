using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Salida
    {
        [Key]
        public int IdSalida { get; set; }

        [ForeignKey("CanalSolicitud")]
        public int IdCanal { get; set; }

        [Required]
        public string CodigoUnico { get; set; } = null!;

        public string? NumeroTicket { get; set; }

        [ForeignKey("UsuarioDestino")]
        public int? IdUsuarioDestino { get; set; }

        [ForeignKey("ParqueaderoDestino")]
        public int? IdParqueaderoDestino { get; set; }

        [ForeignKey("UsuarioEntrega")]
        public int IdUsuarioEntrega { get; set; }

        public DateTime FechaSalida { get; set; }
        public string RegistroSalida { get; set; } = null!;
        public string? Observaciones { get; set; }

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual Usuarios? UsuarioDestino { get; set; }
        public virtual Parqueadero? ParqueaderoDestino { get; set; }
        public virtual Usuarios UsuarioEntrega { get; set; } = null!;
        public virtual Canal CanalSolicitud { get; set; } = null!;
        public virtual ICollection<DetalleSalida> DetallesSalida { get; set; } = new List<DetalleSalida>();
        public virtual ICollection<HistorialActivo> HistorialActivos { get; set; } = new List<HistorialActivo>();
    }
}
