﻿// <auto-generated />
using System;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Biblioteca.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20241017232010_InitialCreate4")]
    partial class InitialCreate4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Biblioteca.Models.Autor", b =>
                {
                    b.Property<Guid>("AutorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Pais")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sobrenome")
                        .HasColumnType("TEXT");

                    b.HasKey("AutorId");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("Biblioteca.Models.Livro", b =>
                {
                    b.Property<string>("LivroId")
                        .HasColumnType("TEXT");

                    b.Property<int>("AnoLancamento")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("TEXT");

                    b.Property<string>("Editora")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genero")
                        .HasColumnType("TEXT");

                    b.Property<int>("QtdExemplares")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .HasColumnType("TEXT");

                    b.HasKey("LivroId");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("Biblioteca.Models.LivroAutor", b =>
                {
                    b.Property<string>("LivroId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AutorId")
                        .HasColumnType("TEXT");

                    b.HasKey("LivroId", "AutorId");

                    b.HasIndex("AutorId");

                    b.ToTable("LivrosAutores");
                });

            modelBuilder.Entity("Biblioteca.Models.LivroAutor", b =>
                {
                    b.HasOne("Biblioteca.Models.Autor", "Autor")
                        .WithMany("LivrosAutores")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Models.Livro", "Livro")
                        .WithMany("LivrosAutores")
                        .HasForeignKey("LivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("Biblioteca.Models.Autor", b =>
                {
                    b.Navigation("LivrosAutores");
                });

            modelBuilder.Entity("Biblioteca.Models.Livro", b =>
                {
                    b.Navigation("LivrosAutores");
                });
#pragma warning restore 612, 618
        }
    }
}