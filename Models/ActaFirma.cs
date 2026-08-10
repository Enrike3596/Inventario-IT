using System.ComponentModel.DataAnnotations;
using Enums;

namespace Models
{
    public class ActaFirma
    {
        [Key]
        public int IdActa { get; set; }

        public string TipoDestino { get; set; } = null!;
        public int IdDestino { get; set; }

        public string? RutaPdf { get; set; }

        public string Token { get; set; } = null!;

        public EstadoActa Estado { get; set; } = EstadoActa.Pendiente;

        public bool Activa { get; set; } = true;

        public DateTime FechaGeneracion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaFirma { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public string? NombreFirmante { get; set; }
        public string? DocumentoFirmante { get; set; }
        public string? IpFirma { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
