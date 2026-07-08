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
        public DbSet<Sedes> Sedes { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<OrdenCompra> OrdenesCompra { get; set; }
        public DbSet<CategoriaActivo> CategoriasActivo { get; set; }
        public DbSet<Activos> Activos { get; set; }
        public DbSet<ItemOC> ItemsOC { get; set; }
        public DbSet<DetalleItemOC> DetallesItemOC { get; set; }
        public DbSet<Parqueadero> Parqueaderos { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<Canal> Canales { get; set; }
        public DbSet<DetalleSalida> DetallesSalida { get; set; }
        public DbSet<AsignacionUsuario> AsignacionesUsuario { get; set; }
        public DbSet<HistorialActivo> HistorialActivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var estadoActivoConverter = new EnumToStringConverter<EstadoActivo>();
            var estadoAsignacionConverter = new EnumToStringConverter<EstadoAsignacion>();
            var estadoUsuarioConverter = new EnumToStringConverter<EstadoUsuario>();
            var tipoMovimientoConverter = new EnumToStringConverter<TipoMovimiento>();
            var estadoGenericoConverter = new EnumToStringConverter<EstadoGenerico>();

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

            modelBuilder.Entity<Sedes>(entity =>
            {
                entity.Property(e => e.Estado).HasConversion(estadoGenericoConverter).HasMaxLength(20);
            });

            // Configuración Salida - Destino obligatorio (al menos uno)
            modelBuilder.Entity<Salida>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_Salida_Destino",
                    "\"IdUsuarioDestino\" IS NOT NULL OR \"IdParqueaderoDestino\" IS NOT NULL"));
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

            // Configuración Salida - Índice único para CódigoUnico
            modelBuilder.Entity<Salida>(entity =>
            {
                entity.HasIndex(s => s.CodigoUnico).IsUnique();
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

            // Configuración OrdenCompra - NumeroOC único
            modelBuilder.Entity<OrdenCompra>(entity =>
            {
                entity.HasIndex(o => o.NumeroOC).IsUnique();
            });

            // Relaciones Salida -> Usuario destino (opcional)
            modelBuilder.Entity<Salida>()
                .HasOne(s => s.UsuarioDestino)
                .WithMany(u => u.SalidasDestino)
                .HasForeignKey(s => s.IdUsuarioDestino)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Salida -> Parqueadero destino (opcional)
            modelBuilder.Entity<Salida>()
                .HasOne(s => s.ParqueaderoDestino)
                .WithMany(p => p.Salidas)
                .HasForeignKey(s => s.IdParqueaderoDestino)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Salida -> Usuario entrega
            modelBuilder.Entity<Salida>()
                .HasOne(s => s.UsuarioEntrega)
                .WithMany(u => u.SalidasEntrega)
                .HasForeignKey(s => s.IdUsuarioEntrega)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Salida -> Canal
            modelBuilder.Entity<Salida>()
                .HasOne(s => s.CanalSolicitud)
                .WithMany(c => c.Salidas)
                .HasForeignKey(s => s.IdCanal)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Relaciones Activos -> OrdenCompra
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.OrdenCompra)
                .WithMany()
                .HasForeignKey(a => a.IdOrden)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Activo
            modelBuilder.Entity<AsignacionUsuario>()
                .HasOne(au => au.ActivoNav)
                .WithMany(a => a.AsignacionesUsuario)
                .HasForeignKey(au => au.IdActivo)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones AsignacionUsuario -> Usuario
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
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones HistorialActivo -> UsuarioEntrega
            modelBuilder.Entity<HistorialActivo>()
                .HasOne(h => h.UsuarioEntrega)
                .WithMany()
                .HasForeignKey(h => h.IdUsuarioEntrega)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones ItemOC -> OrdenCompra
            modelBuilder.Entity<ItemOC>()
                .HasOne(i => i.OrdenCompra)
                .WithMany(o => o.ItemsOC)
                .HasForeignKey(i => i.IdOrden)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones ItemOC -> Categoria
            modelBuilder.Entity<ItemOC>()
                .HasOne(i => i.Categoria)
                .WithMany(c => c.ItemsOC)
                .HasForeignKey(i => i.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones DetalleItemOC -> ItemOC
            modelBuilder.Entity<DetalleItemOC>()
                .HasOne(d => d.ItemOC)
                .WithMany(i => i.DetallesItem)
                .HasForeignKey(d => d.IdItemOC)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones DetalleItemOC -> Activo (opcional)
            modelBuilder.Entity<DetalleItemOC>()
                .HasOne(d => d.Activo)
                .WithMany()
                .HasForeignKey(d => d.IdActivo)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones Activos -> ItemOC
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.ItemOC)
                .WithMany(i => i.Activos)
                .HasForeignKey(a => a.IdItemOC)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Activos -> DetalleItemOC
            modelBuilder.Entity<Activos>()
                .HasOne(a => a.DetalleItemOC)
                .WithMany()
                .HasForeignKey(a => a.IdDetalleItemOC)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Parqueadero -> Sede
            modelBuilder.Entity<Parqueadero>()
                .HasOne(p => p.Sede)
                .WithMany(s => s.Parqueaderos)
                .HasForeignKey(p => p.IdSede)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Usuario -> Rol
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones Usuario -> Sede
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Sede)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.IdSede)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de auditoría - CreadoPor / ModificadoPor
            ConfigureAuditRelationships<Activos>(modelBuilder);
            ConfigureAuditRelationships<AsignacionUsuario>(modelBuilder);
            ConfigureAuditRelationships<Canal>(modelBuilder);
            ConfigureAuditRelationships<CategoriaActivo>(modelBuilder);
            ConfigureAuditRelationships<DetalleItemOC>(modelBuilder);
            ConfigureAuditRelationships<DetalleSalida>(modelBuilder);
            ConfigureAuditRelationships<HistorialActivo>(modelBuilder);
            ConfigureAuditRelationships<ItemOC>(modelBuilder);
            ConfigureAuditRelationships<OrdenCompra>(modelBuilder);
            ConfigureAuditRelationships<Parqueadero>(modelBuilder);
            ConfigureAuditRelationships<Roles>(modelBuilder);
            ConfigureAuditRelationships<Salida>(modelBuilder);
            ConfigureAuditRelationships<Sedes>(modelBuilder);
            ConfigureAuditRelationships<Usuarios>(modelBuilder);
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
