using fleteApi.Models;
using Microsoft.EntityFrameworkCore;

namespace fleteApi.Context
{
    public class FleteContext : DbContext
    {
        public FleteContext() { }

        public FleteContext(DbContextOptions<FleteContext> options)
            : base(options)
        {
        }

        public DbSet<Localidad> Localidades { get; set; } = null!;
        public DbSet<Viaje> Viajes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Localidad_pk");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Viaje>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Viaje_pk");

                entity.Property(e => e.Barrio)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Completado).HasColumnType("datetime");

                entity.Property(e => e.FechaEntrega).HasColumnType("datetime");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Envia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Recibe)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
