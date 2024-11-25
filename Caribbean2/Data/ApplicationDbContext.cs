using Caribbean2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Caribbean2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets para cada tabla
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<Metrica> Metricas { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
                .HasKey(u => u.UsuarioID);

            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<Usuarios>()
                .Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Usuarios>()
                .Property(u => u.Contrasena)
                .IsRequired();

            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios) // Cambiado de Empleados a Usuarios
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);


            // Suscripcion
            modelBuilder.Entity<Suscripcion>()
                .HasKey(b => b.IdSuscripcion);
            modelBuilder.Entity<Suscripcion>()
                .Property(b => b.IdSuscripcion)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Suscripcion>()
                .HasIndex(b => b.Email)
                .IsUnique();
            modelBuilder.Entity<Suscripcion>()
                .Property(s => s.FechaSuscripcion)
                .HasDefaultValueSql("GETDATE()");

            // Empleados
            modelBuilder.Entity<Empleado>()
                .HasKey(e => e.IdEmpleado);
            modelBuilder.Entity<Empleado>()
                .Property(e => e.IdEmpleado)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.EmailEmpleado)
                .IsUnique();

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Rol)
                .WithMany(r => r.Empleados)
                .HasForeignKey(e => e.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Roles
            modelBuilder.Entity<Rol>()
                .HasKey(r => r.IdRol);
            modelBuilder.Entity<Rol>()
                .Property(r => r.IdRol)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.NombreRol)
                .IsUnique();
            modelBuilder.Entity<Rol>()
                .Property(r => r.DescripcionRol)
                .HasMaxLength(255);

            // Permisos
            modelBuilder.Entity<Permiso>()
                .HasKey(p => p.IdPermiso);
            modelBuilder.Entity<Permiso>()
                .Property(p => p.IdPermiso)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Permiso>()
                .HasIndex(p => p.NombrePermiso)
                .IsUnique();

            // Relación muchos a muchos entre Roles y Permisos
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Permisos)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany().HasForeignKey("IdPermiso"),
                    p => p.HasOne<Rol>().WithMany().HasForeignKey("IdRol")
                );

            // Configuración de la relación 1 a N entre Rol y Empleado
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Empleados)
                .WithOne(e => e.Rol)
                .HasForeignKey(e => e.RolId)
                .OnDelete(DeleteBehavior.Restrict); // Evita eliminación en cascada de empleados si el rol es eliminado


            base.OnModelCreating(modelBuilder);
        }
    }
}