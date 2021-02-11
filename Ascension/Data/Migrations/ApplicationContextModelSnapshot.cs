﻿// <auto-generated />
using System;
using System.Text.Json;
using Ascension.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ascension.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Ascension.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("SuperCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SuperCategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Ascension.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("Bytes")
                        .HasColumnType("bytea");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Ascension.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<JsonDocument>("Specifications")
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Ascension.Models.Specification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Specification");
                });

            modelBuilder.Entity("Ascension.Models.SpecificationOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("SpecificationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SpecificationId");

                    b.ToTable("SpecificationOption");
                });

            modelBuilder.Entity("Ascension.Models.SuperCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SuperCategory");
                });

            modelBuilder.Entity("ProductSpecificationOption", b =>
                {
                    b.Property<int>("ProductsId")
                        .HasColumnType("integer");

                    b.Property<int>("SpecificationOptionsId")
                        .HasColumnType("integer");

                    b.HasKey("ProductsId", "SpecificationOptionsId");

                    b.HasIndex("SpecificationOptionsId");

                    b.ToTable("ProductSpecificationOption");
                });

            modelBuilder.Entity("Ascension.Models.Category", b =>
                {
                    b.HasOne("Ascension.Models.SuperCategory", "SuperCategory")
                        .WithMany("Categories")
                        .HasForeignKey("SuperCategoryId");

                    b.Navigation("SuperCategory");
                });

            modelBuilder.Entity("Ascension.Models.Image", b =>
                {
                    b.HasOne("Ascension.Models.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Ascension.Models.Product", b =>
                {
                    b.HasOne("Ascension.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ascension.Models.Specification", b =>
                {
                    b.HasOne("Ascension.Models.Category", "Category")
                        .WithMany("Specifications")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ascension.Models.SpecificationOption", b =>
                {
                    b.HasOne("Ascension.Models.Specification", "Specification")
                        .WithMany("SpecificationOptions")
                        .HasForeignKey("SpecificationId");

                    b.Navigation("Specification");
                });

            modelBuilder.Entity("ProductSpecificationOption", b =>
                {
                    b.HasOne("Ascension.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ascension.Models.SpecificationOption", null)
                        .WithMany()
                        .HasForeignKey("SpecificationOptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ascension.Models.Category", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Specifications");
                });

            modelBuilder.Entity("Ascension.Models.Product", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Ascension.Models.Specification", b =>
                {
                    b.Navigation("SpecificationOptions");
                });

            modelBuilder.Entity("Ascension.Models.SuperCategory", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
