using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.IO;
using Infosys.EncryptDecrypt;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Infosys.QuickKart.DataAccessLayer.Models
{
    public partial class QuickKartDBContext : DbContext
    {
        public QuickKartDBContext()
        {
        }

        public QuickKartDBContext(DbContextOptions<QuickKartDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        //Below function used to call UDF - Scalar Function - Step 1
        public static string ufn_GenerateNewProductId()
        {
            return null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();//Configure the above changes
            var connectionString = config.GetConnectionString("QuickKartConStr");

            var dataSource = config.GetValue<string>("ConnectionParameters:DataSource");
            var database = config.GetValue<string>("ConnectionParameters:Database");

            if (!optionsBuilder.IsConfigured)
            {
                //Decrypt Data Source
                var ds = Repository.Decrypt(dataSource, "ServerKey");
                //Decrypt Database
                var db = Repository.Decrypt(database, "DatabaseKey");

                optionsBuilder.UseSqlServer(string.Format(connectionString, ds, db));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapping SQL Scalar Funtion to Static Method ufn_GenerateNewProductId - Step 2
            modelBuilder.HasDefaultSchema("dbo")
                        .HasDbFunction(() => QuickKartDBContext.ufn_GenerateNewProductId()) //Static Function created in the similar UDF Name
                        .HasName("ufn_GenerateNewProductId");//SQL Server UDF -Scalar Function

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("pk_CategoryId");

                entity.HasIndex(e => e.CategoryName)
                    .HasName("uq_CategoryName")
                    .IsUnique();

                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("pk_ProductId");

                entity.HasIndex(e => e.ProductName)
                    .HasName("uq_ProductName")
                    .IsUnique();

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Price).HasColumnType("numeric(8, 0)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_CategoryId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
