using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class ActivoCreateDTO
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La orden de compra es obligatoria")]
        public int IdOrden { get; set; }

        public int? IdItemOC { get; set; }

        public int? IdDetalleItemOC { get; set; }

        public string? CodigoActivo { get; set; }

        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        public string? Referencia { get; set; }

        public DateTime FechaAdquisicion { get; set; } = DateTime.UtcNow;

        public string? Observaciones { get; set; }
    }

    public class ActivoUpdateDTO
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La orden de compra es obligatoria")]
        public int IdOrden { get; set; }

        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        public string Modelo { get; set; } = null!;

        public string? Referencia { get; set; }

        public EstadoActivo EstadoActivo { get; set; } = EstadoActivo.Disponible;

        public DateTime? FechaBaja { get; set; }

        public string? Observaciones { get; set; }
    }

    public class ActivoResponseDTO
    {
        public int IdActivo { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public int IdOrden { get; set; }
        public string? NumeroOC { get; set; }
        public int? IdItemOC { get; set; }
        public int? IdDetalleItemOC { get; set; }
        public string CodigoActivo { get; set; } = null!;
        public string Serial { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string? Referencia { get; set; }
        public EstadoActivo EstadoActivo { get; set; }
        public DateTime FechaAdquisicion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? CreadoPor { get; set; }
        public int? ModificadoPor { get; set; }
    }
}
