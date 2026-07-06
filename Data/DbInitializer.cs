using Microsoft.EntityFrameworkCore;

namespace Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db, ILogger logger)
        {
            try
            {
                if (db.Database.GetPendingMigrations().Any())
                {
                    logger.LogInformation("Aplicando migraciones pendientes...");
                    db.Database.Migrate();
                }
                else if (!db.Database.CanConnect())
                {
                    logger.LogInformation("Creando base de datos...");
                    db.Database.EnsureCreated();
                }
                else
                {
                    db.Database.Migrate();
                }

                if (!db.Roles.Any())
                {
                    logger.LogInformation("Sembrando datos iniciales...");
                    var sqlPath = Path.Combine(AppContext.BaseDirectory, "Data", "seed.sql");
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
