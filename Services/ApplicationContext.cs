using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WriteDry.Services
{
    public partial class ApplicationContext : DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderCompound> OrderCompounds { get; set; }

        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

        public virtual DbSet<Orderproduct> Orderproducts { get; set; }

        public virtual DbSet<Pcategory> Pcategories { get; set; }

        public virtual DbSet<Pmanufacturer> Pmanufacturers { get; set; }

        public virtual DbSet<Pname> Pnames { get; set; }

        public virtual DbSet<Point> Points { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Provider> Providers { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=trade", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

        public Task EnsureConnectionAsync() => Database.EnsureCreatedAsync();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PRIMARY");

                entity.ToTable("order");

                entity.HasIndex(e => e.OrderPickupPoint, "order_pickuppoint_fk_idx");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");
                entity.Property(e => e.OrderFullname)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.OrderStatus)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.OrderPickupPointNavigation).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderPickupPoint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("order_pickuppoint_fk");
            });

            modelBuilder.Entity<OrderCompound>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.Compound })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("order_compound");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("order_id");
                entity.Property(e => e.Compound)
                    .HasMaxLength(80)
                    .HasColumnName("compound");
                entity.Property(e => e.Amount).HasColumnName("amount");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("order_status");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Orderproduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity
                    .ToTable("orderproduct");

                entity.HasIndex(e => e.ProductArticleNumber, "ProductArticleNumber");

                entity.HasIndex(e => e.OrderId, "orderproduct_fk_idx");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.ProductArticleNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.Order).WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderproduct_fk");

                entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany()
                    .HasForeignKey(d => d.ProductArticleNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderproduct_ibfk_2");
            });

            modelBuilder.Entity<Pcategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("pcategory");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Pmanufacturer>(entity =>
            {
                entity.HasKey(e => e.PmanufacturerId).HasName("PRIMARY");

                entity.ToTable("pmanufacturer");

                entity.Property(e => e.PmanufacturerId).HasColumnName("PManufacturerID");
                entity.Property(e => e.ProductManufacturer)
                    .IsRequired()
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<Pname>(entity =>
            {
                entity.HasKey(e => e.PnameId).HasName("PRIMARY");

                entity.ToTable("pname");
                
                entity.Property(e => e.PnameId).HasColumnName("PNameID");
                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.HasKey(e => e.PointId).HasName("PRIMARY");

                entity.ToTable("point");

                entity.Property(e => e.PointId).ValueGeneratedNever();
                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");
                entity.Property(e => e.House).HasColumnName("house");
                entity.Property(e => e.Index).HasColumnName("index");
                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .HasColumnName("street");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

                entity.ToTable("product");

                entity.HasIndex(e => e.ProductCategory, "product_category_fk_idx");

                entity.HasIndex(e => e.ProductName, "product_fk_idx");

                entity.HasIndex(e => e.ProductManufacturer, "product_manufacture_fk_idx");

                entity.HasIndex(e => e.ProductProvider, "provider_fk_idx");

                entity.HasIndex(e => e.Unit, "unit_fk_idx");

                entity.Property(e => e.ProductArticleNumber)
                    .HasMaxLength(100)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.ProductPhoto)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.Property(e => e.ProductStatus).HasColumnType("text");

                entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_category_fk");

                entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductManufacturer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_manufacture_fk");

                entity.HasOne(d => d.ProductNameNavigation).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("product_fk");

                entity.HasOne(d => d.ProductProviderNavigation).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductProvider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("provider_fk");

                entity.HasOne(d => d.UnitNavigation).WithMany(p => p.Products)
                    .HasForeignKey(d => d.Unit)
                    .HasConstraintName("unit_fk");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("provider");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProviderName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("provider_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PRIMARY");

                entity.ToTable("role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("unit");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("unit_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.UserRole, "user_ibfk_1");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.UserLogin)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnType("text");
                entity.Property(e => e.UserPatronymic)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.UserSurname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
