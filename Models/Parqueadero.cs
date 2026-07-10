using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Models
{
    public class Parqueadero
    {
        [Key]
        public int IdParqueadero { get; set; }

        [ForeignKey("Sede")]
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;
        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual Sedes Sede { get; set; } = null!;
        public virtual ICollection<Salida> Salidas { get; set; } = new List<Salida>();
        public virtual ICollection<AsignacionUsuario> AsignacionesUsuario { get; set; } = new List<AsignacionUsuario>();
    }
}
