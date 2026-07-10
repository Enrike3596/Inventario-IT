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

        [ForeignKey("OrdenCompra")]
        public int IdOrden { get; set; }

        [ForeignKey("ItemOC")]
        public int? IdItemOC { get; set; }

        [ForeignKey("DetalleItemOC")]
        public int? IdDetalleItemOC { get; set; }

        public string CodigoActivo { get; set; } = null!;
        public string Serial { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string? Referencia { get; set; }
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
        public virtual OrdenCompra OrdenCompra { get; set; } = null!;
        public virtual ItemOC? ItemOC { get; set; }
        public virtual DetalleItemOC? DetalleItemOC { get; set; }
        public virtual ICollection<AsignacionUsuario> AsignacionesUsuario { get; set; } = new List<AsignacionUsuario>();
        public virtual ICollection<HistorialActivo> HistorialActivos { get; set; } = new List<HistorialActivo>();
        public virtual ICollection<DetalleSalida> DetallesSalida { get; set; } = new List<DetalleSalida>();
    }
}
