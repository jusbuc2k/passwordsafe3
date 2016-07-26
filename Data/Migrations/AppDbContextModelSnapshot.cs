using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication.Data;

namespace passwordsafe3.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("WebApplication.Data.Password", b =>
                {
                    b.Property<int>("PasswordID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("VaultID");

                    b.HasKey("PasswordID");

                    b.HasIndex("VaultID");

                    b.ToTable("Password");
                });

            modelBuilder.Entity("WebApplication.Data.Vault", b =>
                {
                    b.Property<int>("VaultID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("VaultID");

                    b.ToTable("Vault");
                });

            modelBuilder.Entity("WebApplication.Data.VaultUserKey", b =>
                {
                    b.Property<int>("VaultID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Hash");

                    b.Property<byte[]>("MasterKey");

                    b.Property<string>("Username");

                    b.Property<int?>("VaultID1");

                    b.HasKey("VaultID");

                    b.HasIndex("VaultID1");

                    b.ToTable("VaultUserKey");
                });

            modelBuilder.Entity("WebApplication.Data.Password", b =>
                {
                    b.HasOne("WebApplication.Data.Vault", "Vault")
                        .WithMany()
                        .HasForeignKey("VaultID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication.Data.VaultUserKey", b =>
                {
                    b.HasOne("WebApplication.Data.Vault", "Vault")
                        .WithMany()
                        .HasForeignKey("VaultID1");
                });
        }
    }
}
