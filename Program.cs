using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Middleware;
using Repositories;
using Services;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
builder.Services.AddScoped<ISedeRepository, SedeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaActivoRepository, CategoriaActivoRepository>();
builder.Services.AddScoped<IOrdenCompraRepository, OrdenCompraRepository>();
builder.Services.AddScoped<IActivoRepository, ActivoRepository>();
builder.Services.AddScoped<IParqueaderoRepository, ParqueaderoRepository>();
builder.Services.AddScoped<ICanalRepository, CanalRepository>();
builder.Services.AddScoped<ISalidaRepository, SalidaRepository>();
builder.Services.AddScoped<IDetalleSalidaRepository, DetalleSalidaRepository>();
builder.Services.AddScoped<IHistorialActivoRepository, HistorialActivoRepository>();
builder.Services.AddScoped<IAsignacionUsuarioRepository, AsignacionUsuarioRepository>();

builder.Services.AddScoped<IActivoService, ActivoService>();
builder.Services.AddScoped<IAsignacionUsuarioService, AsignacionUsuarioService>();
builder.Services.AddScoped<ICanalService, CanalService>();
builder.Services.AddScoped<ICategoriaActivoService, CategoriaActivoService>();
builder.Services.AddScoped<IDetalleSalidaService, DetalleSalidaService>();
builder.Services.AddScoped<IHistorialActivoService, HistorialActivoService>();
builder.Services.AddScoped<IOrdenCompraService, OrdenCompraService>();
builder.Services.AddScoped<IParqueaderoService, ParqueaderoService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ISalidaService, SalidaService>();
builder.Services.AddScoped<ISedeService, SedeService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddOpenApi();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

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

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("PermitirFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
