using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class Sedes
    {
        [Key]
        public int IdSede { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public EstadoGenerico Estado { get; set; } = EstadoGenerico.Activo;

        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
        public virtual ICollection<Parqueadero> Parqueaderos { get; set; } = new List<Parqueadero>();
    }
}
