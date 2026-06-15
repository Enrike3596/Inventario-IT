namespace Helpers
{
    public class ResponseHelper
    {
        public static object Success(object? data = null, string? mensaje = null)
        {
            return new { exito = true, data, mensaje };
        }

        public static object Created(object? data = null, string? mensaje = "Recurso creado exitosamente.")
        {
            return new { exito = true, data, mensaje };
        }

        public static object NotFound(string? mensaje = "Recurso no encontrado.")
        {
            return new { exito = false, mensaje };
        }

        public static object Error(string mensaje, object? errores = null)
        {
            return new { exito = false, mensaje, errores };
        }

        public static object BadRequest(string mensaje, object? errores = null)
        {
            return new { exito = false, mensaje, errores };
        }
    }
}
