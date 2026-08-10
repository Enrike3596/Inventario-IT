using DTOs;

namespace Services.ActaFirma
{
    public interface IActaFirmaService
    {
        Task<ActaFirmaResponseDTO> GenerarActaAsync(int idDestino, string tipoDestino);
        Task<ActaFirmaResponseDTO> EnviarParaFirmaAsync(int idDestino, string tipoDestino);
        Task<ActaFirmaPublicDTO?> ObtenerPorTokenAsync(string token);
        Task<ActaFirmaResponseDTO> FirmarAsync(string token, FirmaRequestDTO dto, string ipAddress);
        Task<ActaFirmaResponseDTO?> ObtenerPorDestinoAsync(int idDestino, string tipoDestino);
        Task EliminarActaAsync(int idDestino, string tipoDestino);
    }
}
