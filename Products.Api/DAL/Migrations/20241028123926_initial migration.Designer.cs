﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Products.Api.DAL;

#nullable disable

namespace Products.Api.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241028123926_initial migration")]
    partial class initialmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Products.Api.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductEntity", b =>
                {
                    b.Property<string>("ArticleNumber")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductGroupId")
                        .HasColumnType("uuid");

                    b.HasKey("ArticleNumber");

                    b.HasIndex("ProductGroupId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductGroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryName");

                    b.ToTable("ProductGroups");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductVariationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductArticleNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Stock")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductArticleNumber");

                    b.ToTable("ProductVariations");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductEntity", b =>
                {
                    b.HasOne("Products.Api.Entities.ProductGroupEntity", "ProductGroup")
                        .WithMany("Products")
                        .HasForeignKey("ProductGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductGroup");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductGroupEntity", b =>
                {
                    b.HasOne("Products.Api.Entities.CategoryEntity", "Category")
                        .WithMany("ProductGroups")
                        .HasForeignKey("CategoryName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductVariationEntity", b =>
                {
                    b.HasOne("Products.Api.Entities.ProductEntity", "Product")
                        .WithMany("Variations")
                        .HasForeignKey("ProductArticleNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Products.Api.Entities.CategoryEntity", b =>
                {
                    b.Navigation("ProductGroups");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductEntity", b =>
                {
                    b.Navigation("Variations");
                });

            modelBuilder.Entity("Products.Api.Entities.ProductGroupEntity", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
