using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Services
{
    public class ReporteExportador : IReporteExportador
    {
        private const string ColorPurple = "#552373";
        private const string ColorMagenta = "#B80E80";
        private const string ColorNavy = "#263D77";
        private const string ColorTeal = "#009BAA";
        private const string ColorTealClaro = "#EAF6F7";
        private const string ColorFondo = "#FCF9FF";

        private static readonly Lazy<byte[]?> _logoBytes = new(() =>
        {
            try
            {
                var logoPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "Logo INDIGO ORG. 2.png");
                return File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
            }
            catch { return null; }
        });

        public byte[] GenerarPdf(List<string> columnas, List<Dictionary<string, object?>> filas)
        {
            var fechaGeneracion = DateTime.Now;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(24);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));
                    page.PageColor(ColorFondo);

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Background(ColorPurple).Height(6);
                            row.RelativeItem().Background(ColorTeal).Height(6);
                        });

                        if (_logoBytes.Value != null)
                        {
                            col.Item().PaddingVertical(10).AlignCenter().Width(95)
                                .Image(_logoBytes.Value).FitWidth();
                        }

                        col.Item().AlignCenter().Text("INFORME DE INVENTARIO")
                            .Bold().FontSize(18).FontColor(ColorMagenta);
                        col.Item().AlignCenter()
                            .Text($"Generado el {fechaGeneracion:dd/MM/yyyy HH:mm} — {filas.Count} registros")
                            .FontSize(11).FontColor(ColorNavy);
                        col.Item().PaddingVertical(8).LineHorizontal(1).LineColor(ColorTeal);
                    });

                    page.Content().PaddingTop(8).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            foreach (var _ in columnas) cols.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (var col in columnas)
                                header.Cell().Border(0.5f).BorderColor(ColorTeal)
                                    .Background(ColorPurple).Padding(5)
                                    .Text(col).SemiBold().FontSize(9).FontColor(Colors.White).AlignCenter();
                        });

                        for (int i = 0; i < filas.Count; i++)
                        {
                            var bg = i % 2 == 0 ? "#FFFFFF" : ColorTealClaro;
                            foreach (var col in columnas)
                                table.Cell().Border(0.5f).BorderColor("#009BAA40").Background(bg).Padding(5)
                                    .Text(filas[i].GetValueOrDefault(col)?.ToString() ?? "-").FontSize(9);
                        }
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingVertical(6).LineHorizontal(0.5f).LineColor("#009BAA40");
                        col.Item().AlignCenter().DefaultTextStyle(x => x.FontSize(8).FontColor("#555555"))
                            .Text(t =>
                            {
                                t.Span("Generado el ").SemiBold();
                                t.Span(fechaGeneracion.ToString("yyyy-MM-dd HH:mm"));
                                t.Span(" — Página ");
                                t.CurrentPageNumber();
                                t.Span(" de ");
                                t.TotalPages();
                            });
                    });
                });
            });

            return documento.GeneratePdf();
        }

        public byte[] GenerarExcel(List<string> columnas, List<Dictionary<string, object?>> filas)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Inventario");

            var fechaGeneracion = DateTime.Now;
            var totalColumnas = Math.Max(1, columnas.Count);

            // Banner título
            sheet.Range(1, 1, 1, totalColumnas).Merge();
            sheet.Cell(1, 1).Value = "INFORME DE INVENTARIO";
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 14;
            sheet.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml(ColorPurple);
            sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Row(1).Height = 42;

            // Banner subtítulo
            sheet.Range(2, 1, 2, totalColumnas).Merge();
            sheet.Cell(2, 1).Value = $"Generado el {fechaGeneracion:dd/MM/yyyy HH:mm} — {filas.Count} registros";
            sheet.Cell(2, 1).Style.Font.FontSize = 10;
            sheet.Cell(2, 1).Style.Font.FontColor = XLColor.White;
            sheet.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.FromHtml(ColorTeal);
            sheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Row(2).Height = 20;

            const int headerRow = 3;

            // Logo en el extremo izquierdo del banner
            if (_logoBytes.Value != null)
            {
                using var logoStream = new MemoryStream(_logoBytes.Value);
                var picture = sheet.AddPicture(logoStream);
                picture.MoveTo(sheet.Cell(1, 1));
                picture.WithSize(110, 38);
            }

            // Encabezados
            for (int i = 0; i < columnas.Count; i++)
            {
                var cell = sheet.Cell(headerRow, i + 1);
                cell.Value = columnas[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(ColorPurple);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.FromHtml(ColorTeal);
            }

            // Filas de datos
            for (int f = 0; f < filas.Count; f++)
            {
                var bg = f % 2 == 0 ? XLColor.White : XLColor.FromHtml(ColorTealClaro);
                for (int c = 0; c < columnas.Count; c++)
                {
                    var cell = sheet.Cell(f + headerRow + 1, c + 1);
                    cell.Value = filas[f].GetValueOrDefault(columnas[c])?.ToString() ?? "-";
                    cell.Style.Fill.BackgroundColor = bg;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#009BAA40");
                }
            }

            sheet.Columns().AdjustToContents();
            sheet.SheetView.FreezeRows(headerRow);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
