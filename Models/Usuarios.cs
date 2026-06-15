using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Models
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [ForeignKey("Rol")]
        public int IdRol { get; set; }

        [ForeignKey("Sede")]
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Cargo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public EstadoUsuario EstadoUsuario { get; set; } = EstadoUsuario.Activo;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public virtual Roles Rol { get; set; } = null!;
        public virtual Sedes Sede { get; set; } = null!;
        public virtual ICollection<Salida> SalidasEntrega { get; set; } = new List<Salida>();
        public virtual ICollection<Salida> SalidasDestino { get; set; } = new List<Salida>();
        public virtual ICollection<AsignacionUsuario> Asignaciones { get; set; } = new List<AsignacionUsuario>();
    }
}
