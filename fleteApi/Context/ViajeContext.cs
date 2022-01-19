using Microsoft.EntityFrameworkCore;

namespace fleteApi.Models
{
    public class ViajeContext : DbContext
    {
        public ViajeContext()
        {
        }

        public ViajeContext(DbContextOptions<ViajeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Viaje> Viaje { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Viaje>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Viaje_pk")
                    .IsClustered(false);

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
        }
    }
}
