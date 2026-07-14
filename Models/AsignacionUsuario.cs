using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Models
{
    public class AsignacionUsuario
    {
        [Key]
        public int IdAsignacion { get; set; }

        [ForeignKey("ActivoNav")]
        public int IdActivo { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuarioDestino { get; set; }

        [ForeignKey("Parqueadero")]
        public int? IdParqueadero { get; set; }

        [ForeignKey("CanalSolicitud")]
        public int IdCanal { get; set; }

        [ForeignKey("UsuarioEntrega")]
        public int IdUsuarioEntrega { get; set; }

        public string RegistroSalida { get; set; } = null!;
        public string? NumeroTicket { get; set; }

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
        public EstadoAsignacion EstadoAsignacion { get; set; } = EstadoAsignacion.Activa;

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual Activos ActivoNav { get; set; } = null!;
        public virtual Usuarios Usuario { get; set; } = null!;
        public virtual Parqueadero? Parqueadero { get; set; }
        public virtual Canal CanalSolicitud { get; set; } = null!;
        public virtual Usuarios UsuarioEntrega { get; set; } = null!;
    }
}
