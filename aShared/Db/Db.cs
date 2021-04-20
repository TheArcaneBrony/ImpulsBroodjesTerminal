using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ThumbnailGenerator
{
    public partial class Db : DbContext
    {
        public Db()
        {
        }

        public Db(DbContextOptions<Db> options)
            : base(options)
        {
        }

        public virtual DbSet<Broodje> Broodjes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=thearcanebrony.net;database=impulsbroodjes;uid=impulsbroodjes_demo;pwd=impulsbroodjes", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.23-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Broodje>(entity =>
            {
                entity.ToTable("broodjes");

                entity.HasIndex(e => e.BroodjeIngredients, "broodje_ingredienten_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.BroodjeName, "broodje_naam_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.BroodjeId).HasColumnName("broodje_id");

                entity.Property(e => e.BroodjeImage)
                    .HasColumnType("varchar(500)")
                    .HasColumnName("broodje_image")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.BroodjeImageThumbnail1024)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("broodje_image_thumbnail_1024")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.BroodjeImageThumbnail128)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("broodje_image_thumbnail_128")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.BroodjeIngredients)
                    .IsRequired()
                    .HasColumnType("varchar(150)")
                    .HasColumnName("broodje_ingredients")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.BroodjeName)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasColumnName("broodje_name")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.BroodjePrice).HasColumnName("broodje_price");

                entity.Property(e => e.BroodjeType)
                    .HasColumnType("varchar(15)")
                    .HasColumnName("broodje_type")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.UserEmail, "user_email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserCanViewOrders)
                    .HasColumnName("user_can_view_orders")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UserEmail)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("user_email")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserEnabled)
                    .HasColumnName("user_enabled")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserFirstname)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("user_firstname")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserIsAdmin)
                    .HasColumnName("user_is_admin")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UserLastname)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("user_lastname")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserPassword)
                    .HasColumnType("varchar(60)")
                    .HasColumnName("user_password")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
