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

        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
        public EstadoAsignacion EstadoAsignacion { get; set; } = EstadoAsignacion.Activa;

        public virtual Activos ActivoNav { get; set; } = null!;
        public virtual Usuarios Usuario { get; set; } = null!;
        public virtual Parqueadero? Parqueadero { get; set; }
    }
}
