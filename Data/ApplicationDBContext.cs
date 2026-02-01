using Microsoft.EntityFrameworkCore;
using Prueba_Completa.Models;

namespace Prueba_Completa.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetalleOrdenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relacion Cliente - Orden
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relacion Orden - DetalleOrden
            modelBuilder.Entity<DetalleOrden>()
                .HasOne(d => d.Orden)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relacion Producto - DetalleOrden
            modelBuilder.Entity<DetalleOrden>()
                .HasOne(d => d.Producto)
                .WithMany(p => p.DetallesOrden)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}