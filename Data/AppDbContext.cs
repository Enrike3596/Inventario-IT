using System.Security.Claims;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Remision> Remisiones { get; set; }
        public DbSet<CategoriaActivo> CategoriasActivo { get; set; }
        public DbSet<Activos> Activos { get; set; }
        public DbSet<ItemRemision> ItemsRemision { get; set; }
        public DbSet<DetalleItemRemision> DetallesItemRemision { get; set; }
        public DbSet<Parqueadero> Parqueaderos { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<Canal> Canales { get; set; }
        public DbSet<DetalleSalida> DetallesSalida { get; set; }
        public DbSet<AsignacionUsuario> AsignacionesUsuario { get; set; }
        public DbSet<HistorialActivo> HistorialActivos { get; set; }
        public DbSet<ActaFirma> ActasFirma { get; set; }
        public DbSet<Area> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var estadoActivoConverter = new EnumToStringConverter<EstadoActivo>();
            var estadoAsignacionConverter = new EnumToStringConverter<EstadoAsignacion>();
            var estadoUsuarioConverter = new EnumToStringConverter<EstadoUsuario>();
            var tipoMovimientoConverter = new EnumToStringConverter<TipoMovimiento>();
            var estadoGenericoConverter = new EnumToStringConverter<EstadoGenerico>();
            var estadoActaConverter = new EnumToStringConverter<EstadoActa>();

            modelBuilder.Entity<Activos>(entity =>
            {
                entity.Property(e => e.EstadoActivo).HasConversion(estadoActivoConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<AsignacionUsuario>(entity =>
            {
                entity.Property(e => e.EstadoAsignacion).HasConversion(estadoAsignacionConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.Property(e => e.EstadoUsuario).HasConversion(estadoUsuarioConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<HistorialActivo>(entity =>
            {
                entity.Property(e => e.TipoMovimiento).HasConversion(tipoMovimientoConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<CategoriaActivo>(entity =>
            {
                entity.Property(e => e.Estado).HasConversion(estadoGenericoConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<Parqueadero>(entity =>
            {
                entity.Property(e => e.Estado).HasConversion(estadoGenericoConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Estado).HasConversion(estadoGenericoConverter).HasMaxLength(20);
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasIndex(a => a.NombreArea).IsUnique();
            });

            // Soft-delete: Estado por defecto activo
            modelBuilder.Entity<Canal>(entity =>
            {
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<Remision>(entity =>
            {
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<ItemRemision>(entity =>
            {
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<DetalleItemRemision>(entity =>
            {
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<Salida>(entity =>
            {
                entity.Property(e => e.Estado).HasDefaultValue(true);
            });

            modelBuilder.Entity<ActaFirma>(entity =>
            {
                entity.Property(e => e.Estado).HasConversion(estadoActaConverter).HasMaxLength(20);
                entity.Property(e => e.Activa).HasDefaultValue(true);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasIndex(e => new { e.IdDestino, e.TipoDestino });
                entity.Property(e => e.TipoDestino).HasMaxLength(20).IsRequired();
            });

            // Configuración Activos - Código único
            modelBuilder.Entity<Activos>(entity =>
            {
                entity.HasIndex(a => a.CodigoActivo).IsUnique();
                entity.HasIndex(a => a.Serial).IsUnique();
            });

            // Configuración AsignacionUsuario - Un activo no puede estar asignado dos veces activamente
            modelBuilder.Entity<AsignacionUsuario>(entity =>
            {
                entity.HasIndex(a => new { a.IdActivo, a.EstadoAsignacion })
                      .HasDatabaseName("IX_Asignacion_ActivoUnico")
                      .IsUnique()
                      .HasFilter("\"EstadoAsignacion\" = 'Activa'");
            });

            // Configuración Salida - Índice único para CódigoUnico + conversión EstadoActivo
            modelBuilder.Entity<Salida>(entity =>
            {
                entity.HasIndex(s => s.CodigoUnico).IsUnique();
                entity.Property(e => e.EstadoActivo).HasConversion(estadoActivoConverter).HasMaxLength(20);
            });

            // Configuración CategoriaActivo - Nombre único
            modelBuilder.Entity<CategoriaActivo>(entity =>
            {
                entity.HasIndex(c => c.Nombre).IsUnique();
            });

            // Configuración Canal - Nombre único
            modelBuilder.Entity<Canal>(entity =>
            {
                entity.HasIndex(c => c.Nombre).IsUnique();
            });

            // Configuración Usuarios - Correo único
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasIndex(u => u.Correo).IsUnique();
            });

            // Configuración Remision - NumeroRemision único
            modelBuilder.Entity<Remision>(entity =>
            {
                entity.HasIndex(r => r.NumeroRemision).IsUnique();
            });

            // Relaciones DetalleSalida -> Salida
            modelBuilder.Entity<DetalleSalida>()
                .HasOne(d => d.Salida)
                .WithMany(s => s.DetallesSalida)
                .HasForeignKey(d => d.IdSalida)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones DetalleSalida -> Activo
            modelBuilder.Entity<DetalleSalida>()
                .HasOne(d => d.Activo)
                .WithMany(a => a.DetallesSalida)
                .HasForeignKey(d => d.IdActivo)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Activos -> Categoria
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.Categoria)
                .WithMany(c => c.Activos)
                .HasForeignKey(a => a.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Activos -> Remision
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.Remision)
                .WithMany()
                .HasForeignKey(a => a.IdRemision)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Activo
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.ActivoNav)
                .WithMany(a => a.AsignacionesUsuario)
                .HasForeignKey(au => au.IdActivo)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Usuario destino
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.Usuario)
                .WithMany(u => u.Asignaciones)
                .HasForeignKey(au => au.IdUsuarioDestino)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Parqueadero (opcional)
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.Parqueadero)
                .WithMany(p => p.AsignacionesUsuario)
                .HasForeignKey(au => au.IdParqueadero)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones AsignacionUsuario -> Canal
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.CanalSolicitud)
                .WithMany(c => c.Asignaciones)
                .HasForeignKey(au => au.IdCanal)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Usuario entrega
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.UsuarioEntrega)
                .WithMany(u => u.AsignacionesEntrega)
                .HasForeignKey(au => au.IdUsuarioEntrega)
                .OnDelete(DeleteBehavior.Restrict);

            // ActaFirma ahora referencia grupo (Usuario/Parqueadero) en lugar de AsignacionUsuario

            // Relaciones HistorialActivo -> Activo
            modelBuilder.Entity<HistorialActivo>()
                .HasOne(h => h.Activo)
                .WithMany(a => a.HistorialActivos)
                .HasForeignKey(h => h.IdActivo)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones HistorialActivo -> Salida
            modelBuilder.Entity<HistorialActivo>()
                .HasOne(h => h.Salida)
                .WithMany(s => s.HistorialActivos)
                .HasForeignKey(h => h.IdSalida)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones HistorialActivo -> UsuarioEntrega
            modelBuilder.Entity<HistorialActivo>()
                .HasOne(h => h.UsuarioEntrega)
                .WithMany()
                .HasForeignKey(h => h.IdUsuarioEntrega)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones HistorialActivo -> AsignacionUsuario
            modelBuilder.Entity<HistorialActivo>()
                .HasOne(h => h.Asignacion)
                .WithMany()
                .HasForeignKey(h => h.IdAsignacion)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones ItemRemision -> Remision
            modelBuilder.Entity<ItemRemision>()
                .HasOne(i => i.Remision)
                .WithMany(r => r.ItemsRemision)
                .HasForeignKey(i => i.IdRemision)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones ItemRemision -> Categoria
            modelBuilder.Entity<ItemRemision>()
                .HasOne(i => i.Categoria)
                .WithMany(c => c.ItemsRemision)
                .HasForeignKey(i => i.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones DetalleItemRemision -> ItemRemision
            modelBuilder.Entity<DetalleItemRemision>()
                .HasOne(d => d.ItemRemision)
                .WithMany(i => i.DetallesItem)
                .HasForeignKey(d => d.IdItemRemision)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones DetalleItemRemision -> Activo (opcional)
            modelBuilder.Entity<DetalleItemRemision>()
                .HasOne(d => d.Activo)
                .WithMany()
                .HasForeignKey(d => d.IdActivo)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones Activos -> ItemRemision
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.ItemRemision)
                .WithMany(i => i.Activos)
                .HasForeignKey(a => a.IdItemRemision)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Activos -> DetalleItemRemision
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.DetalleItemRemision)
                .WithMany()
                .HasForeignKey(a => a.IdDetalleItemRemision)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Parqueadero - DA único
            modelBuilder.Entity<Parqueadero>()
                .HasIndex(p => p.DA)
                .IsUnique()
                .HasDatabaseName("IX_Parqueaderos_DA");

            // Relaciones Usuario -> Rol
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Usuario -> Area (opcional)
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Area)
                .WithMany(a => a.Usuarios)
                .HasForeignKey(u => u.IdArea)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones de auditoría - CreadoPor / ModificadoPor
            ConfigureAuditRelationships<Activos>(modelBuilder);
            ConfigureAuditRelationships<ActaFirma>(modelBuilder);
            ConfigureAuditRelationships<AsignacionUsuario>(modelBuilder);
            ConfigureAuditRelationships<Canal>(modelBuilder);
            ConfigureAuditRelationships<CategoriaActivo>(modelBuilder);
            ConfigureAuditRelationships<DetalleItemRemision>(modelBuilder);
            ConfigureAuditRelationships<DetalleSalida>(modelBuilder);
            ConfigureAuditRelationships<HistorialActivo>(modelBuilder);
            ConfigureAuditRelationships<ItemRemision>(modelBuilder);
            ConfigureAuditRelationships<Remision>(modelBuilder);
            ConfigureAuditRelationships<Parqueadero>(modelBuilder);
            ConfigureAuditRelationships<Roles>(modelBuilder);
            ConfigureAuditRelationships<Salida>(modelBuilder);
            ConfigureAuditRelationships<Usuarios>(modelBuilder);
            ConfigureAuditRelationships<Area>(modelBuilder);
        }

        private static void ConfigureAuditRelationships<T>(ModelBuilder modelBuilder) where T : class
        {
            var entity = modelBuilder.Entity<T>();

            entity.HasOne<Usuarios>()
                .WithMany()
                .HasForeignKey("CreadoPor")
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne<Usuarios>()
                .WithMany()
                .HasForeignKey("ModificadoPor")
                .OnDelete(DeleteBehavior.SetNull);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("FechaCreacion").CurrentValue = DateTime.UtcNow;
                    if (userId.HasValue)
                        entry.Property("CreadoPor").CurrentValue = userId.Value;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("FechaModificacion").CurrentValue = DateTime.UtcNow;
                    entry.Property("FechaCreacion").IsModified = false;
                    if (userId.HasValue)
                        entry.Property("ModificadoPor").CurrentValue = userId.Value;
                    else
                        entry.Property("ModificadoPor").IsModified = false;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private int? GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idClaim, out var id))
                    return id;
            }
            return null;
        }
    }
}
