﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PerguntasRespostas.Infra.Data.Context;

namespace PerguntasRespostas.Infra.Data.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20181026004729_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PerguntasRespostas.Domain.Entities.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao")
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("PerguntasRespostas.Domain.Entities.Pergunta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Autor")
                        .HasColumnType("varchar(150)");

                    b.Property<Guid?>("CategoriaId");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.ToTable("Perguntas");
                });

            modelBuilder.Entity("PerguntasRespostas.Domain.Entities.Respostas", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Autor")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(155)");

                    b.Property<Guid>("PerguntaId");

                    b.HasKey("Id");

                    b.HasIndex("PerguntaId");

                    b.ToTable("Respostas");
                });

            modelBuilder.Entity("PerguntasRespostas.Domain.Entities.Respostas", b =>
                {
                    b.HasOne("PerguntasRespostas.Domain.Entities.Pergunta", "Pergunta")
                        .WithMany("Respostas")
                        .HasForeignKey("PerguntaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
