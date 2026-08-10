namespace Services
{
    public interface IReporteExportador
    {
        byte[] GenerarPdf(List<string> columnas, List<Dictionary<string, object?>> filas);

        byte[] GenerarExcel(List<string> columnas, List<Dictionary<string, object?>> filas);
    }
}
