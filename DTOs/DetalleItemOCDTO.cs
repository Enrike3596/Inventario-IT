using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class DetalleItemOCCreateDTO
    {
        [Required(ErrorMessage = "El item de OC es obligatorio")]
        public int IdItemOC { get; set; }

        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        public string? Observaciones { get; set; }
    }

    public class DetalleItemOCUpdateDTO
    {
        [Required(ErrorMessage = "El serial es obligatorio")]
        public string Serial { get; set; } = null!;

        public string? Observaciones { get; set; }
    }

    public class DetalleItemOCResponseDTO
    {
        public int IdDetalleItemOC { get; set; }
        public int IdItemOC { get; set; }
        public string Serial { get; set; } = null!;
        public bool Procesado { get; set; }
        public int? IdActivo { get; set; }
        public string? CodigoActivo { get; set; }
        public string? Observaciones { get; set; }
    }

    public class DetalleItemOCBatchCreateDTO
    {
        [Required(ErrorMessage = "El item de OC es obligatorio")]
        public int IdItemOC { get; set; }

        [Required(ErrorMessage = "Debe ingresar al menos un serial")]
        public List<string> Seriales { get; set; } = new();
    }
}
