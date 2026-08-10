using System.Security.Cryptography;
using DTOs;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Repositories;
using Services.Emails;
using Services.FileStorage;

namespace Services.ActaFirma
{
    public class FirmaElectronicaSettings
    {
        public string FrontendBaseUrl { get; set; } = "http://localhost:5173";
        public int TokenExpirationHours { get; set; } = 72;
    }

    public class ActaFirmaService : IActaFirmaService
    {
        private readonly IActaFirmaRepository _repo;
        private readonly IFileStorageService _fileStorage;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FirmaElectronicaSettings _settings;
        private readonly Data.AppDbContext _context;

        public ActaFirmaService(
            IActaFirmaRepository repo,
            IFileStorageService fileStorage,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IOptions<FirmaElectronicaSettings> settings,
            Data.AppDbContext context)
        {
            _repo = repo;
            _fileStorage = fileStorage;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings.Value;
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<ActaFirmaResponseDTO> GenerarActaAsync(int idDestino, string tipoDestino)
        {
            var asignaciones = await ObtenerAsignacionesActivasAsync(idDestino, tipoDestino);
            if (asignaciones.Count == 0)
                throw new InvalidOperationException("No hay asignaciones activas para este destino.");

            var existente = await _repo.ObtenerPorDestinoAsync(idDestino, tipoDestino);
            if (existente is { Estado: EstadoActa.Pendiente or EstadoActa.Enviada })
                throw new InvalidOperationException("Ya existe un acta activa para este destino.");

            var (nombreDestino, _) = await ObtenerInfoDestinoAsync(idDestino, tipoDestino);

            var acta = new Models.ActaFirma
            {
                TipoDestino = tipoDestino,
                IdDestino = idDestino,
                Token = GenerarToken(),
                Estado = EstadoActa.Pendiente,
                FechaGeneracion = DateTime.UtcNow,
                FechaVencimiento = DateTime.UtcNow.AddHours(_settings.TokenExpirationHours)
            };

            var pdfBytes = GenerarPdf(asignaciones, acta, nombreDestino);
            using var stream = new MemoryStream(pdfBytes);
            var ruta = await _fileStorage.SaveAsync("actas", $"acta_{tipoDestino}_{idDestino}.pdf", stream);
            acta.RutaPdf = ruta;

            await _repo.CrearAsync(acta);

            var urlPdf = await _fileStorage.GetUrl(ruta);
            return MapToDTO(acta, urlPdf, nombreDestino, asignaciones);
        }

        public async Task<ActaFirmaResponseDTO> EnviarParaFirmaAsync(int idDestino, string tipoDestino)
        {
            var acta = await _repo.ObtenerPorDestinoAsync(idDestino, tipoDestino)
                ?? throw new InvalidOperationException("No hay acta generada para este destino. Genere el acta primero.");

            if (acta.Estado == EstadoActa.Firmada)
                throw new InvalidOperationException("El acta ya fue firmada.");

            if (acta.FechaVencimiento < DateTime.UtcNow)
            {
                acta.Estado = EstadoActa.Vencida;
                await _repo.ActualizarAsync(acta);
                throw new InvalidOperationException("El acta ha vencido. Genere una nueva.");
            }

            var asignaciones = await ObtenerAsignacionesActivasAsync(idDestino, tipoDestino);
            var (nombreDestino, usuarioDestino) = await ObtenerInfoDestinoAsync(idDestino, tipoDestino);

            if (usuarioDestino == null || string.IsNullOrWhiteSpace(usuarioDestino.Correo))
                throw new InvalidOperationException("El usuario destino no tiene correo electrónico registrado.");

            var firmaLink = $"{_settings.FrontendBaseUrl}/firmar/{acta.Token}";

            var activosHtml = string.Join("", asignaciones.Select((a, i) =>
                $"<tr style='background:{(i % 2 == 0 ? "#ffffff" : "#f8f9fa")};border-bottom:1px solid #e8e8eb'>" +
                $"<td style='padding:14px 16px;color:#555555;font-size:14px;font-weight:500'>{i + 1}</td>" +
                $"<td style='padding:14px 16px;color:#333333;font-size:14px;font-family:monospace;font-weight:500'>{a.ActivoNav?.Serial ?? "—"}</td>" +
                $"<td style='padding:14px 16px;color:#333333;font-size:14px'>{a.ActivoNav?.Marca ?? "—"} {a.ActivoNav?.Modelo ?? "—"}</td></tr>"));

            var subject = "Acta de asignación de activos - Firma electrónica pendiente";
            var body = $"""
                <!DOCTYPE html>
                <html lang="es">
                <head>
                    <meta charset="utf-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                </head>
                <body style="margin:0;padding:0;background-color:#f4f4f7;font-family:'Segoe UI',Tahoma,Geneva,Verdana,sans-serif">
                    <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#f4f4f7">
                        <tr>
                            <td align="center" style="padding:40px 20px">
                                <table role="presentation" width="600" cellpadding="0" cellspacing="0" style="background-color:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 20px rgba(0,0,0,0.08)">
                                    <!-- Header -->
                                    <tr>
                                        <td style="background:linear-gradient(135deg,#552373 0%,#B80E80 100%);padding:35px 40px;text-align:center">
                                            <h1 style="margin:0;color:#ffffff;font-size:24px;font-weight:600;letter-spacing:0.5px">Acta de Asignación de Activos</h1>
                                            <p style="margin:8px 0 0;color:rgba(255,255,255,0.85);font-size:13px">Sistema de Gestión de Inventario TI</p>
                                        </td>
                                    </tr>
                                    <!-- Body -->
                                    <tr>
                                        <td style="padding:40px">
                                            <p style="margin:0 0 20px;color:#333333;font-size:15px">Hola <strong style="color:#552373">{usuarioDestino.Nombre}</strong>,</p>
                                            <p style="margin:0 0 25px;color:#555555;font-size:14px;line-height:1.6">
                                                Se te {((tipoDestino == "Parqueadero") ? "han asignado los siguientes activos" : "ha asignado el siguiente activo")}. 
                                                Por favor, revisa los detalles a continuación y firma electrónicamente para confirmar la recepción.
                                            </p>
                                            <!-- Tabla de activos -->
                                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="border-collapse:collapse;margin-bottom:30px;border-radius:8px;overflow:hidden;border:1px solid #e8e8eb">
                                                <thead>
                                                    <tr>
                                                        <th style="background:#552373;color:#ffffff;padding:14px 16px;text-align:left;font-size:13px;font-weight:600;text-transform:uppercase;letter-spacing:0.5px">#</th>
                                                        <th style="background:#552373;color:#ffffff;padding:14px 16px;text-align:left;font-size:13px;font-weight:600;text-transform:uppercase;letter-spacing:0.5px">Serial</th>
                                                        <th style="background:#552373;color:#ffffff;padding:14px 16px;text-align:left;font-size:13px;font-weight:600;text-transform:uppercase;letter-spacing:0.5px">Activo</th>
                                                    </tr>
                                                </thead>
                                                <tbody>{activosHtml}</tbody>
                                            </table>
                                            <!-- Botón CTA -->
                                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center" style="padding:10px 0 25px">
                                                        <a href="{firmaLink}" style="display:inline-block;background:linear-gradient(135deg,#552373 0%,#B80E80 100%);color:#ffffff;padding:16px 48px;border-radius:8px;text-decoration:none;font-size:15px;font-weight:600;letter-spacing:0.3px;box-shadow:0 4px 15px rgba(184,14,128,0.35);transition:all 0.3s ease">
                                                            ✍️ Firmar Acta
                                                        </a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- Info de expiración -->
                                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background-color:#fff8e1;border-radius:8px;border-left:4px solid #ffb300">
                                                <tr>
                                                    <td style="padding:16px 20px">
                                                        <p style="margin:0;color:#5d4037;font-size:13px;line-height:1.5">
                                                            <strong>⏰ IMPORTANTE:</strong> Este enlace expira el <strong>{acta.FechaVencimiento:dd/MM/yyyy HH:mm} UTC</strong>. 
                                                            Si no reconoces esta solicitud, ignora este correo.
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <!-- Footer -->
                                    <tr>
                                        <td style="background-color:#f8f9fa;padding:25px 40px;border-top:1px solid #e8e8eb">
                                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align:center">
                                                        <p style="margin:0 0 5px;color:#888888;font-size:12px">Sistema de Gestión de Inventario TI</p>
                                                        <p style="margin:0;color:#aaaaaa;font-size:11px">Este es un correo generado automáticamente, por favor no responder.</p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>
                """;

            await _emailSender.SendAsync(usuarioDestino.Correo, subject, body);

            acta.Estado = EstadoActa.Enviada;
            acta.FechaEnvio = DateTime.UtcNow;
            await _repo.ActualizarAsync(acta);

            var urlPdf = await _fileStorage.GetUrl(acta.RutaPdf!);
            return MapToDTO(acta, urlPdf, nombreDestino, asignaciones);
        }

        public async Task<ActaFirmaPublicDTO?> ObtenerPorTokenAsync(string token)
        {
            var acta = await _repo.ObtenerPorTokenAsync(token);
            if (acta == null) return null;

            if (acta.FechaVencimiento < DateTime.UtcNow && acta.Estado != EstadoActa.Firmada)
            {
                acta.Estado = EstadoActa.Vencida;
                await _repo.ActualizarAsync(acta);
            }

            var asignaciones = await ObtenerAsignacionesActivasAsync(acta.IdDestino, acta.TipoDestino);
            var (nombreDestino, _) = await ObtenerInfoDestinoAsync(acta.IdDestino, acta.TipoDestino);

            var primera = asignaciones.FirstOrDefault();

            return new ActaFirmaPublicDTO
            {
                IdActa = acta.IdActa,
                Token = acta.Token,
                Estado = acta.Estado,
                FechaFirma = acta.FechaFirma,
                NombreFirmante = acta.NombreFirmante,
                TipoDestino = acta.TipoDestino,
                IdDestino = acta.IdDestino,
                NombreDestino = nombreDestino,
                NombreUsuarioEntrega = primera?.UsuarioEntrega?.Nombre,
                FechaAsignacion = primera?.FechaAsignacion ?? DateTime.UtcNow,
                RegistroSalida = primera?.RegistroSalida ?? "",
                Activos = asignaciones.Select(a => new ActaActivoDTO
                {
                    IdActivo = a.IdActivo,
                    CodigoActivo = a.ActivoNav?.CodigoActivo,
                    Serial = a.ActivoNav?.Serial,
                    Marca = a.ActivoNav?.Marca,
                    Modelo = a.ActivoNav?.Modelo,
                    NombreCategoria = a.ActivoNav?.Categoria?.Nombre,
                }).ToList(),
            };
        }

        public async Task<ActaFirmaResponseDTO> FirmarAsync(string token, FirmaRequestDTO dto, string ipAddress)
        {
            var acta = await _repo.ObtenerPorTokenAsync(token)
                ?? throw new InvalidOperationException("Token inválido.");

            if (acta.Estado == EstadoActa.Firmada)
                throw new InvalidOperationException("El acta ya fue firmada anteriormente.");

            if (acta.FechaVencimiento < DateTime.UtcNow)
            {
                acta.Estado = EstadoActa.Vencida;
                await _repo.ActualizarAsync(acta);
                throw new InvalidOperationException("El enlace ha expirado. Solicita un nuevo enlace.");
            }

            if (acta.Estado != EstadoActa.Enviada)
                throw new InvalidOperationException("El acta aún no ha sido enviada para firma.");

            acta.Estado = EstadoActa.Firmada;
            acta.FechaFirma = DateTime.UtcNow;
            acta.NombreFirmante = dto.Nombre;
            acta.DocumentoFirmante = dto.Documento;
            acta.IpFirma = ipAddress;

            await _repo.ActualizarAsync(acta);

            var asignaciones = await ObtenerAsignacionesActivasAsync(acta.IdDestino, acta.TipoDestino);
            var (nombreDestino, _) = await ObtenerInfoDestinoAsync(acta.IdDestino, acta.TipoDestino);
            var urlPdf = await _fileStorage.GetUrl(acta.RutaPdf!);
            return MapToDTO(acta, urlPdf, nombreDestino, asignaciones);
        }

        public async Task<ActaFirmaResponseDTO?> ObtenerPorDestinoAsync(int idDestino, string tipoDestino)
        {
            var acta = await _repo.ObtenerPorDestinoAsync(idDestino, tipoDestino);
            if (acta == null) return null;

            var asignaciones = await ObtenerAsignacionesActivasAsync(idDestino, tipoDestino);
            var (nombreDestino, _) = await ObtenerInfoDestinoAsync(idDestino, tipoDestino);
            var urlPdf = acta.RutaPdf != null ? await _fileStorage.GetUrl(acta.RutaPdf) : null;
            return MapToDTO(acta, urlPdf, nombreDestino, asignaciones);
        }

        public async Task EliminarActaAsync(int idDestino, string tipoDestino)
        {
            var acta = await _repo.ObtenerPorDestinoAsync(idDestino, tipoDestino)
                ?? throw new InvalidOperationException("No hay acta para este destino.");

            if (acta.Estado == EstadoActa.Firmada)
                throw new InvalidOperationException("No se puede eliminar un acta que ya fue firmada.");

            if (acta.RutaPdf != null)
            {
                var parts = acta.RutaPdf.Split('/', 2);
                if (parts.Length == 2)
                    await _fileStorage.DeleteAsync(parts[0], parts[1]);
            }

            await _repo.EliminarAsync(acta);
        }

        // ─── helpers ───────────────────────────────────────────────

        private async Task<List<AsignacionUsuario>> ObtenerAsignacionesActivasAsync(int idDestino, string tipoDestino)
        {
            IQueryable<AsignacionUsuario> query = _context.AsignacionesUsuario
                .Include(a => a.ActivoNav).ThenInclude(ac => ac!.Categoria)
                .Include(a => a.Usuario)
                .Include(a => a.UsuarioEntrega)
                .Include(a => a.CanalSolicitud)
                .Where(a => a.EstadoAsignacion == EstadoAsignacion.Activa);

            if (tipoDestino == "Usuario")
                query = query.Where(a => a.IdUsuarioDestino == idDestino);
            else
                query = query.Where(a => a.IdParqueadero == idDestino);

            return await query.ToListAsync();
        }

        private async Task<(string nombre, Usuarios? usuario)> ObtenerInfoDestinoAsync(int idDestino, string tipoDestino)
        {
            if (tipoDestino == "Usuario")
            {
                var user = await _context.Usuarios.FindAsync(idDestino);
                return (user?.Nombre ?? "—", user);
            }
            else
            {
                var parq = await _context.Parqueaderos.FindAsync(idDestino);
                return (parq?.Nombre ?? "—", null);
            }
        }

        private static ActaFirmaResponseDTO MapToDTO(
            Models.ActaFirma acta,
            string? urlPdf,
            string? nombreDestino,
            List<AsignacionUsuario> asignaciones)
        {
            return new ActaFirmaResponseDTO
            {
                IdActa = acta.IdActa,
                RutaPdf = acta.RutaPdf,
                UrlPdf = urlPdf,
                Token = acta.Token,
                Estado = acta.Estado,
                FechaGeneracion = acta.FechaGeneracion,
                FechaEnvio = acta.FechaEnvio,
                FechaFirma = acta.FechaFirma,
                FechaVencimiento = acta.FechaVencimiento,
                NombreFirmante = acta.NombreFirmante,
                DocumentoFirmante = acta.DocumentoFirmante,
                IpFirma = acta.IpFirma,
                TipoDestino = acta.TipoDestino,
                IdDestino = acta.IdDestino,
                NombreDestino = nombreDestino,
                Activos = asignaciones.Select(a => new ActaActivoDTO
                {
                    IdActivo = a.IdActivo,
                    CodigoActivo = a.ActivoNav?.CodigoActivo,
                    Serial = a.ActivoNav?.Serial,
                    Marca = a.ActivoNav?.Marca,
                    Modelo = a.ActivoNav?.Modelo,
                    NombreCategoria = a.ActivoNav?.Categoria?.Nombre,
                }).ToList(),
            };
        }

        private static string GenerarToken()
        {
            var bytes = new byte[32];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private static readonly Lazy<byte[]?> _logoBytes = new(() =>
        {
            try
            {
                var logoPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\", "Logo INDIGO ORG. 2.png");
                return File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;
            }
            catch { return null; }
        });

        private byte[] GenerarPdf(
            List<AsignacionUsuario> asignaciones,
            Models.ActaFirma acta,
            string? nombreDestino)
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));
                    page.PageColor("#FCF9FF");

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Background("#552373").Height(6);
                            row.RelativeItem().Background("#009BAA").Height(6);
                        });

                        if (_logoBytes.Value != null)
                        {
                            col.Item().PaddingVertical(14).AlignCenter().Width(95)
                                .Image(_logoBytes.Value).FitWidth();
                        }

                        col.Item().AlignCenter().Text("ACTA DE ASIGNACIÓN DE ACTIVOS")
                            .Bold().FontSize(18).FontColor("#B80E80");
                        col.Item().AlignCenter().Text($"N° {acta.IdActa}")
                            .FontSize(13).FontColor("#263D77");
                        col.Item().PaddingVertical(8).LineHorizontal(1).LineColor("#009BAA");
                    });

                    page.Content().Column(col =>
                    {
                        var primera = asignaciones.First();
                        col.Item().PaddingBottom(12).Text(t =>
                        {
                            t.Span("Fecha: ").Bold();
                            t.Span(acta.FechaGeneracion.ToString("dd/MM/yyyy"));
                        });

                        col.Item().PaddingBottom(4)
                            .Text("DESTINO").Bold().FontSize(13).FontColor("#B80E80");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1);
                                c.RelativeColumn(2);
                            });
                            void DestCell(string label, string value)
                            {
                                table.Cell().Border(0.5f).BorderColor("#009BAA30").Padding(6)
                                    .Text(label).SemiBold().FontSize(10);
                                table.Cell().Border(0.5f).BorderColor("#009BAA30").Padding(6)
                                    .Text(value).FontSize(10);
                            }
                            DestCell(acta.TipoDestino == "Usuario" ? "Usuario" : "Parqueadero", nombreDestino ?? "—");
                            DestCell("Entrega", primera.UsuarioEntrega?.Nombre ?? "—");
                            DestCell("Canal", primera.CanalSolicitud?.Nombre ?? "—");
                            DestCell("Reg. salida", primera.RegistroSalida);
                        });

                        col.Item().PaddingTop(12).PaddingBottom(4)
                            .Text("ACTIVOS ASIGNADOS").Bold().FontSize(13).FontColor("#B80E80");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(0.5f);
                                c.RelativeColumn(1.5f);
                                c.RelativeColumn(1.2f);
                                c.RelativeColumn(1.2f);
                                c.RelativeColumn(1.2f);
                            });

                            void Header(string label)
                            {
                                table.Cell().Border(0.5f).BorderColor("#55237340")
                                    .Background("#009BAA12").Padding(5)
                                    .Text(label).SemiBold().FontSize(9).AlignCenter();
                            }
                            Header("#");
                            Header("Serial");
                            Header("Marca");
                            Header("Modelo");
                            Header("Categoría");

                            for (int i = 0; i < asignaciones.Count; i++)
                            {
                                var a = asignaciones[i];
                                var bg = i % 2 == 0 ? "#FFFFFF" : "#009BAA08";
                                void Cell(string val)
                                {
                                    table.Cell().Border(0.5f).BorderColor("#009BAA30")
                                        .Background(bg).Padding(5)
                                        .Text(val ?? "—").FontSize(9);
                                }
                                Cell((i + 1).ToString());
                                Cell(a.ActivoNav?.Serial ?? "—");
                                Cell(a.ActivoNav?.Marca ?? "—");
                                Cell(a.ActivoNav?.Modelo ?? "—");
                                Cell(a.ActivoNav?.Categoria?.Nombre ?? "—");
                            }
                        });

                        col.Item().PaddingTop(12).LineHorizontal(0.5f).LineColor("#009BAA40");

                        col.Item().PaddingTop(10).PaddingBottom(4)
                            .Text("* Esta información será suministrada y/o recibida por el subproceso de Servicios Administrativos")
                            .FontSize(8).FontColor("#555555").Italic();

                        col.Item().PaddingTop(6).PaddingBottom(4)
                            .Text(t =>
                            {
                                t.Span("El proceso de Tecnología de la Información y el Grupo INDIGO ha determinado que se debe utilizar el repositorio en la nube con el proveedor de dominio definido, para el respaldo de toda la información de la compañía.").FontSize(8);
                            });

                        col.Item().PaddingTop(4).PaddingBottom(4)
                            .Text(t =>
                            {
                                t.Span("El colaborador es responsable del uso, manejo y respaldo de la información que deje alojada en su equipo tecnológico (Disco duro, escritorio, descargas, etc.), así como la pérdida que pudiese presentarse en caso de daño, intromisión, hackeo, etc. De acuerdo con lo descrito anteriormente, el proceso de Tecnología de la Información NO será responsable por situaciones que se presenten por este motivo.").FontSize(8);
                            });

                        col.Item().PaddingTop(4).PaddingBottom(8)
                            .Text(t =>
                            {
                                t.Span("Nota: Solo se realizará backup de la información que cumpla con las características de seguridad de la información determinadas por la compañía de acuerdo con lo descrito anteriormente").FontSize(8).Italic();
                            });

                        col.Item().PaddingTop(6).PaddingBottom(4)
                            .Text("Compromiso del Colaborador").Bold().FontSize(10).FontColor("#552373");
                        col.Item().PaddingBottom(4)
                            .Text("Cuando se recibe el equipo descrito en la presente acta manifiesta que se encuentra en condiciones adecuadas de funcionamiento y se compromete a:")
                            .FontSize(8);

                        string[] compromisosColaborador = new[]
                        {
                            "Usar los equipos y recursos tecnológicos asignados única y exclusivamente para el cumplimiento de las funciones laborales.",
                            "Mantener y conservar el equipo en buen estado, los accesorios, el software autorizado y demás elementos entregados por la compañía.",
                            "No instalar software, aplicaciones o herramientas no autorizadas por el proceso de Tecnología de la Información.",
                            "Preservar la confidencialidad de toda la información a la que tenga acceso en el cumplimiento de sus funciones laborales.",
                            "Informar oportunamente por medio de correo electrónico al responsable de la entrega de equipos o recursos tecnológicos asignados cualquier incidente, pérdida, robo, daño, acceso no autorizado o falla.",
                            "Permitir las revisiones, inventarios y verificaciones al equipo que el proceso de Tecnología de la Información considere necesarias.",
                            "Realizar la devolución del equipo, accesorios, credenciales y demás elementos asignados cuando sea requerido por la compañía o al finalizar la relación laboral o contractual.",
                            "Cuando se presenten daños ocasionados por golpes, caídas, derrame de líquidos, pérdida, hurto por descuido o cualquier situación derivada de un uso inadecuado o negligente por parte del colaborador, acepto que la compañía tome las acciones correspondientes, una vez el proceso de Tecnología de la Información haya realizado verificación de la información suministrada por el colaborador, la revisión de los hechos y los soportes a que haya lugar.",
                            "El colaborador deberá comunicar por medio de correo electrónico la situación que se presentó, comprobar las gestiones que realizó ante la situación presentada y suministrar el soporte de la Denuncia Virtual realizada ante la Policía Nacional."
                        };
                        for (int i = 0; i < compromisosColaborador.Length; i++)
                        {
                            col.Item().PaddingBottom(2).Row(row =>
                            {
                                row.ConstantItem(18).Text($"{i + 1}.").FontSize(8).FontColor("#552373");
                                row.RelativeItem().Text(compromisosColaborador[i]).FontSize(8);
                            });
                        }

                        col.Item().PaddingTop(6).PaddingBottom(4)
                            .Text("Cuando al colaborador le sea requerido por parte de la compañía o al finalizar su relación laboral o contractual; deberá entregar los elementos descritos en la presente acta y garantizar la firma del PAZ Y SALVO FO-CP-GH-AP-012 (006), teniendo en cuenta:")
                            .FontSize(8);

                        string[] entregaElementos = new[]
                        {
                            "Devolver el equipo tecnológico, los accesorios y demás elementos e información alojada en él, en buen estado al responsable asignado (Bogotá) y/o jefe directo (a nivel nacional).",
                            "Informar cualquier novedad presentada durante el uso de los elementos, tales como daños, deterioro, pérdida o hurto.",
                            "Hacer entrega de las credenciales entendiendo que son de propiedad de la compañía y que su uso no está autorizado fuera de la relación laboral o contractual.",
                            "Permitir la verificación del estado de los elementos por parte del Asistente Administrativo TI o Coordinador Servicios Administrativos (dependiendo del equipo) al momento de la devolución.",
                            "Garantizar que se suscriba esta acta como evidencia de aceptación frente a su entrega."
                        };
                        for (int i = 0; i < entregaElementos.Length; i++)
                        {
                            col.Item().PaddingBottom(2).Row(row =>
                            {
                                row.ConstantItem(18).Text($"{i + 1}.").FontSize(8).FontColor("#552373");
                                row.RelativeItem().Text(entregaElementos[i]).FontSize(8);
                            });
                        }

                        col.Item().PaddingTop(6).PaddingBottom(4)
                            .Text("Compromiso del Responsable de Entrega/ Recepción").Bold().FontSize(10).FontColor("#552373");
                        col.Item().PaddingBottom(4)
                            .Text("Los únicos responsables de la aceptación de recibo y/o entrega de estos elementos son:")
                            .FontSize(8);

                        string[] responsables = new[]
                        {
                            "Los celulares: Coordinador Servicios Administrativos o Jefe Administrativa (cuando el primero no esté).",
                            "Los equipos tecnológicos: Asistente Administrativo TI o Coordinador de Postventa (cuando el primero no esté) o Coordinador de Desarrollo e Integración de Software (cuando el primero y el segundo no esté)."
                        };
                        foreach (var r in responsables)
                        {
                            col.Item().PaddingBottom(2).Row(row =>
                            {
                                row.ConstantItem(10).Text("•").FontSize(8).FontColor("#009BAA");
                                row.RelativeItem().Text(r).FontSize(8);
                            });
                        }

                        col.Item().PaddingTop(4).PaddingBottom(4)
                            .Text("Estos tienen la responsabilidad de diligenciar esta acta en la entrega y/o recepción de los equipos tecnológicos y las credenciales de acceso, así como garantizar su entrega al Asistente de Contratación para su almacenamiento en la carpeta física del colaborador.")
                            .FontSize(8);

                        col.Item().PaddingTop(6).PaddingBottom(4)
                            .Text("Cuando se realice la entrega de los elementos descritos anteriormente en ciudades diferentes a Bogotá D.C, la recepción de estos la realizará el jefe inmediato; sin embargo, los responsables anteriormente descritos deberán enviar vía correo esta acta diligenciada, para la firma del colaborador y garantizar que retorne suscrita por ambas partes en un plazo máximo de 15 días hábiles. En caso que esto no se cumpla, quedará como responsable el jefe inmediato, hasta tanto no se haga entrega y aceptación en la oficina principal.")
                            .FontSize(8);

                        col.Item().PaddingTop(4).PaddingBottom(4)
                            .Text("Cuando se devuelva por parte del colaborador los elementos descritos anteriormente en ciudades diferentes a Bogotá D.C., el recibo de estos la realizará el jefe inmediato, no obstante, deberán en un plazo máximo de 10 días hábiles enviar un correo electrónico con el acta escaneada, firmada por el jefe inmediato y el colaborador e incluir:")
                            .FontSize(8);

                        string[] ciudadesDev = new[]
                        {
                            "Celulares: Registro fotográfico del elemento.",
                            "Equipos tecnológicos: Envío físico del elemento a la oficina principal para su revisión y firma de aceptación del PAZ Y SALVO FO-CP-GH-AP-012 (006)."
                        };
                        foreach (var c in ciudadesDev)
                        {
                            col.Item().PaddingBottom(2).Row(row =>
                            {
                                row.ConstantItem(10).Text("•").FontSize(8).FontColor("#009BAA");
                                row.RelativeItem().Text(c).FontSize(8);
                            });
                        }

                        col.Item().PaddingTop(6).PaddingBottom(10)
                            .Text("Con la firma de la presente acta, el colaborador y el responsable de entrega o recepción, se comprometen y aceptan las condiciones de los activos tecnológicos suministrados anteriormente.")
                            .FontSize(8).SemiBold();

                        col.Item().PaddingTop(20).PaddingBottom(4)
                            .Text("FIRMAS").Bold().FontSize(13).FontColor("#B80E80");
                        col.Item().PaddingTop(20).Row(row =>
                        {
                            row.RelativeItem().PaddingRight(15).Column(c2 =>
                            {
                                c2.Item().LineHorizontal(1).LineColor("#263D77");
                                c2.Item().PaddingTop(8).AlignCenter().Text("ENTREGA").SemiBold().FontSize(10).FontColor("#552373");
                                c2.Item().AlignCenter().Text(primera.UsuarioEntrega?.Nombre ?? "—")
                                    .FontSize(9).FontColor("#263D77");
                            });
                            row.RelativeItem().PaddingLeft(15).Column(c2 =>
                            {
                                c2.Item().LineHorizontal(1).LineColor("#263D77");
                                c2.Item().PaddingTop(8).AlignCenter().Text("RECIBE").SemiBold().FontSize(10).FontColor("#552373");
                                if (acta.Estado == EstadoActa.Firmada)
                                {
                                    c2.Item().AlignCenter().Text(acta.NombreFirmante ?? "—")
                                        .FontSize(9).FontColor("#009BAA");
                                    c2.Item().AlignCenter().Text($"Doc: {acta.DocumentoFirmante}")
                                        .FontSize(8).FontColor("#263D77");
                                    c2.Item().AlignCenter().Text($"Firmado: {acta.FechaFirma:dd/MM/yyyy HH:mm}")
                                        .FontSize(8).FontColor("#263D77");
                                }
                                else
                                {
                                    c2.Item().AlignCenter().Text(nombreDestino ?? "—")
                                        .FontSize(9).FontColor("#263D77");
                                }
                            });
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().AlignCenter().DefaultTextStyle(x => x.FontSize(8).FontColor("#263D77"))
                            .Text($"Documento generado electrónicamente - Fecha: {acta.FechaGeneracion:dd/MM/yyyy}");
                        col.Item().AlignCenter().DefaultTextStyle(x => x.FontSize(8).FontColor("#B80E8080"))
                            .Text("Sistema de Gestión de Inventario TI");
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
