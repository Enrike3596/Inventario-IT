using Microsoft.EntityFrameworkCore;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db, ILogger logger, string contentRootPath)
        {
            try
            {
                if (db.Database.CanConnect())
                {
                    logger.LogInformation("Aplicando migraciones pendientes...");
                    db.Database.Migrate();
                }
                else
                {
                    logger.LogInformation("Creando base de datos y aplicando migraciones...");
                    db.Database.Migrate();
                }

                if (!db.Roles.Any())
                {
                    logger.LogInformation("Sembrando datos iniciales...");
                    var sqlPath = Path.Combine(contentRootPath, "Data", "seed.sql");
                    if (File.Exists(sqlPath))
                    {
                        var sql = File.ReadAllText(sqlPath);
                        db.Database.ExecuteSqlRaw(sql);
                        logger.LogInformation("Seed ejecutado correctamente.");
                    }
                    else
                    {
                        logger.LogWarning("Archivo seed.sql no encontrado en: {Path}", sqlPath);
                    }
                }
                else
                {
                    logger.LogInformation("La base de datos ya contiene datos.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error durante la inicialización de la base de datos.");
                throw;
            }
        }
    }
}
