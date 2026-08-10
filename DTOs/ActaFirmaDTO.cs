using Enums;

namespace DTOs
{
    public class ActaActivoDTO
    {
        public int IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Serial { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? NombreCategoria { get; set; }
    }

    public class DestinoRequest
    {
        public string TipoDestino { get; set; } = null!;
        public int IdDestino { get; set; }
    }

    public class ActaFirmaResponseDTO
    {
        public int IdActa { get; set; }
        public string? RutaPdf { get; set; }
        public string? UrlPdf { get; set; }
        public string Token { get; set; } = null!;
        public EstadoActa Estado { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaFirma { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string? NombreFirmante { get; set; }
        public string? DocumentoFirmante { get; set; }
        public string? IpFirma { get; set; }

        public string TipoDestino { get; set; } = null!;
        public int IdDestino { get; set; }
        public string? NombreDestino { get; set; }

        public List<ActaActivoDTO> Activos { get; set; } = new();
    }

    public class ActaFirmaPublicDTO
    {
        public int IdActa { get; set; }
        public string Token { get; set; } = null!;
        public EstadoActa Estado { get; set; }
        public bool YaFirmada => Estado == EstadoActa.Firmada;
        public DateTime? FechaFirma { get; set; }
        public string? NombreFirmante { get; set; }

        public string TipoDestino { get; set; } = null!;
        public int IdDestino { get; set; }
        public string? NombreDestino { get; set; }

        public string? NombreUsuarioEntrega { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string RegistroSalida { get; set; } = null!;

        public List<ActaActivoDTO> Activos { get; set; } = new();
    }

    public class FirmaRequestDTO
    {
        public string Nombre { get; set; } = null!;
        public string Documento { get; set; } = null!;
    }
}
