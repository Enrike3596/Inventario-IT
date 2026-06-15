using System.ComponentModel.DataAnnotations;
using Enums;

namespace DTOs
{
    public class AsignacionUsuarioCreateDTO
    {
        [Required(ErrorMessage = "El activo es obligatorio")]
        public int IdActivo { get; set; }

        [Required(ErrorMessage = "El usuario destino es obligatorio")]
        public int IdUsuarioDestino { get; set; }

        public int? IdParqueadero { get; set; }
    }

    public class AsignacionUsuarioUpdateDTO
    {
        public EstadoAsignacion EstadoAsignacion { get; set; } = EstadoAsignacion.Activa;
    }

    public class AsignacionUsuarioResponseDTO
    {
        public int IdAsignacion { get; set; }
        public int IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Serial { get; set; }
        public int IdUsuarioDestino { get; set; }
        public string? NombreUsuarioDestino { get; set; }
        public int? IdParqueadero { get; set; }
        public string? NombreParqueadero { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public EstadoAsignacion EstadoAsignacion { get; set; }
    }
}
