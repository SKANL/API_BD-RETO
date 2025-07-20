using System;
using ApiBD.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBD.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoCaducidad> ProductosCaducidad { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                // Tabla y columnas en snake_case para coincidir con la BD
                entity.ToTable("categorias");
                entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");
                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("nombre_UNIQUE");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                // Tabla y columnas en snake_case
                entity.ToTable("proveedores");
                entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("nombre_UNIQUE");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.HasIndex(e => e.Telefono).IsUnique().HasDatabaseName("telefono_UNIQUE");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("email_UNIQUE");
                entity.Property(e => e.Direccion).HasColumnName("direccion");
                entity.HasIndex(e => e.Direccion).IsUnique().HasDatabaseName("direccion_UNIQUE");
                entity.Property(e => e.DiaRecarga).HasColumnName("dia_recarga");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                // Tabla y columnas en snake_case
                entity.ToTable("productos");
                entity.HasKey(e => e.IdProducto).HasName("PRIMARY");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(255);
                entity.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("nombre_UNIQUE");
                entity.Property(e => e.CodigoDeBarra)
                    .HasColumnName("codigo_de_barra");
                entity.HasIndex(e => e.CodigoDeBarra)
                    .IsUnique()
                    .HasDatabaseName("codigo_de_barra_UNIQUE");
                entity.Property(e => e.PrecioCosto)
                    .HasColumnName("precio_costo")
                    .HasPrecision(10, 2)
                    .IsRequired();
                entity.Property(e => e.PrecioVenta)
                    .HasColumnName("precio_venta")
                    .HasPrecision(10, 2)
                    .IsRequired();
                entity.Property(e => e.StockActual)
                    .HasColumnName("stock_actual")
                    .IsRequired();
                entity.Property(e => e.StockMinimo)
                    .HasColumnName("stock_minimo")
                    .IsRequired();
                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .IsRequired();
                entity.Property(e => e.IdProveedor)
                    .HasColumnName("id_proveedor");
                entity.HasOne(e => e.Categoria)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(e => e.IdCategoria)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Proveedor)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(e => e.IdProveedor)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ProductoCaducidad>(entity =>
            {
                // Tabla y columnas en snake_case
                entity.ToTable("productos_caducidad");
                entity.HasKey(e => e.IdProducto).HasName("PRIMARY");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.FechaCaducidad).HasColumnName("fecha_caducidad").IsRequired();
                entity.HasOne(pc => pc.Producto)
                    .WithOne(p => p.ProductoCaducidad)
                    .HasForeignKey<ProductoCaducidad>(pc => pc.IdProducto)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                // Tabla y columnas en snake_case
                entity.ToTable("ventas");
                entity.HasKey(e => e.IdVenta).HasName("PRIMARY");
                entity.Property(e => e.IdVenta).HasColumnName("id_venta");
                entity.Property(e => e.Fecha).HasColumnName("fecha").IsRequired();
                entity.Property(e => e.Total).HasColumnName("total").HasPrecision(10, 2);
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                // Tabla y columnas en snake_case
                entity.ToTable("detalles_venta");
                entity.HasKey(e => e.IdDetalle).HasName("PRIMARY");
                entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
                entity.Property(e => e.IdVenta).HasColumnName("id_venta");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad").IsRequired();
                entity.Property(e => e.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(10, 2);
                entity.HasOne(d => d.Venta)
                    .WithMany(v => v.DetallesVenta)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
