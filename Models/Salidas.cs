using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class Salida
    {
        [Key]
        public int IdSalida { get; set; }

        [Required]
        public string CodigoUnico { get; set; } = null!;

        public EstadoActivo EstadoActivo { get; set; }

        public DateTime FechaSalida { get; set; }
        public string? Observaciones { get; set; }

        public string? MotivoEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }

        public virtual ICollection<DetalleSalida> DetallesSalida { get; set; } = new List<DetalleSalida>();
        public virtual ICollection<HistorialActivo> HistorialActivos { get; set; } = new List<HistorialActivo>();
    }
}
