using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, title, detail) = ex switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado", ex.Message),
                ArgumentException => (StatusCodes.Status400BadRequest, "Solicitud inválida", ex.Message),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado", ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor", "Ocurrió un error inesperado.")
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Type = $"https://httpstatuses.io/{statusCode}",
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
