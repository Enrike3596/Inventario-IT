using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Middleware;
using QuestPDF.Infrastructure;
using Repositories;
using Services;
using Services.ActaFirma;
using Services.Emails;
using Services.FileStorage;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var problem = new ProblemDetails
            {
                Type = "https://httpstatuses.io/400",
                Title = "Error de validación",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Uno o más campos no son válidos.",
                Instance = context.HttpContext.Request.Path
            };
            problem.Extensions["errors"] = errors;
            problem.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

            return new ObjectResult(problem)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentTypes = { "application/problem+json" }
            };
        };
    });

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaActivoRepository, CategoriaActivoRepository>();
builder.Services.AddScoped<IRemisionRepository, RemisionRepository>();
builder.Services.AddScoped<IActivoRepository, ActivoRepository>();
builder.Services.AddScoped<IItemRemisionRepository, ItemRemisionRepository>();
builder.Services.AddScoped<IDetalleItemRemisionRepository, DetalleItemRemisionRepository>();
builder.Services.AddScoped<IParqueaderoRepository, ParqueaderoRepository>();
builder.Services.AddScoped<ISalidaRepository, SalidaRepository>();
builder.Services.AddScoped<IDetalleSalidaRepository, DetalleSalidaRepository>();
builder.Services.AddScoped<IHistorialActivoRepository, HistorialActivoRepository>();
builder.Services.AddScoped<IAsignacionUsuarioRepository, AsignacionUsuarioRepository>();
builder.Services.AddScoped<IActaFirmaRepository, ActaFirmaRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();

builder.Services.AddScoped<IActivoService, ActivoService>();
builder.Services.AddScoped<IItemRemisionService, ItemRemisionService>();
builder.Services.AddScoped<IDetalleItemRemisionService, DetalleItemRemisionService>();
builder.Services.AddScoped<IAsignacionUsuarioService, AsignacionUsuarioService>();
builder.Services.AddScoped<ICategoriaActivoService, CategoriaActivoService>();
builder.Services.AddScoped<IDetalleSalidaService, DetalleSalidaService>();
builder.Services.AddScoped<IHistorialActivoService, HistorialActivoService>();
builder.Services.AddScoped<IRemisionService, RemisionService>();
builder.Services.AddScoped<IParqueaderoService, ParqueaderoService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ISalidaService, SalidaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.Configure<FirmaElectronicaSettings>(builder.Configuration.GetSection("FirmaElectronica"));
builder.Services.AddScoped<IActaFirmaService, ActaFirmaService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<ReporteInventarioService>();
builder.Services.AddScoped<IReporteExportador, ReporteExportador>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorage"));

builder.Services.AddSingleton<IEmailTemplate, EmailTemplate>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();

// Frena fuerza bruta y credential-stuffing particionando por IP del cliente.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(5),
                QueueLimit = 0
            }));
    options.AddPolicy("firma-publica", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(5),
                QueueLimit = 0
            }));
});

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

// La clave firma todos los JWT: si es débil o falta, los tokens son falsificables.
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Jwt:Key debe configurarse con al menos 256 bits (32 caracteres).");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            // Ventana de tolerancia del reloj corta: evita reutilizar tokens recién expirados.
            ClockSkew = TimeSpan.FromMinutes(2),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
        // Nota: no se acepta el JWT por query string (access_token) para que el token
        // no quede en URLs, logs de servidor/proxy ni historial del navegador.
    });

builder.Services.AddAuthorization(options =>
{
    // Por defecto todo endpoint exige usuario autenticado; lo público se marca
    // explícitamente con [AllowAnonymous] (login, reset, firma pública de actas).
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    DbInitializer.Initialize(db, logger, app.Environment.ContentRootPath);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // En producción todo viaja por HTTPS y el navegador memoriza el uso de TLS.
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Cabeceras de seguridad en todas las respuestas (API y archivos estáticos).
// Los PDF se abren en pestaña nueva, por eso frame-ancestors 'none' es seguro.
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'none'; frame-ancestors 'none'; base-uri 'none'";
    await next();
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("PermitirFrontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

var uploadsPath = Path.GetFullPath(builder.Configuration.GetSection("FileStorage:BasePath").Value ?? "uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = builder.Configuration.GetSection("FileStorage:BaseUrl").Value ?? "/uploads"
});

app.MapControllers();

app.Run();
