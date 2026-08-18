using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

namespace Models
{
    public class Activos
    {
        [Key]
        public int IdActivo { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [ForeignKey("Remision")]
        public int IdRemision { get; set; }

        [ForeignKey("ItemRemision")]
        public int? IdItemRemision { get; set; }

        [ForeignKey("DetalleItemRemision")]
        public int? IdDetalleItemRemision { get; set; }

        public string CodigoActivo { get; set; } = null!;
        public string Serial { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public EstadoActivo EstadoActivo { get; set; } = EstadoActivo.Disponible;
        public DateTime FechaAdquisicion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaBaja { get; set; }
        public string? Observaciones { get; set; }

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual CategoriaActivo Categoria { get; set; } = null!;
        public virtual Remision Remision { get; set; } = null!;
        public virtual ItemRemision? ItemRemision { get; set; }
        public virtual DetalleItemRemision? DetalleItemRemision { get; set; }
        public virtual ICollection<AsignacionUsuario> AsignacionesUsuario { get; set; } = new List<AsignacionUsuario>();
        public virtual ICollection<HistorialActivo> HistorialActivos { get; set; } = new List<HistorialActivo>();
        public virtual ICollection<DetalleSalida> DetallesSalida { get; set; } = new List<DetalleSalida>();
    }
}
