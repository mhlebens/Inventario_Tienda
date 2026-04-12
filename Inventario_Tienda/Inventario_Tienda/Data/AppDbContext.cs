using Inventario_Tienda.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventario_Tienda.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.ToTable("Producto");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.PrecioCompra).HasColumnName("precioCompra");
                entity.Property(e => e.PrecioVenta).HasColumnName("precioVenta");
                entity.Property(e => e.StockActual).HasColumnName("stockActual");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.HasOne(e => e.Categoria)
                      .WithMany()
                      .HasForeignKey(e => e.IdCategoria);

                entity.HasOne(e => e.Proveedor)
                      .WithMany()
                      .HasForeignKey(e => e.IdProveedor);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);
                entity.ToTable("Categoria");
                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);
                entity.ToTable("Proveedor");
                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });
        }
    }
}