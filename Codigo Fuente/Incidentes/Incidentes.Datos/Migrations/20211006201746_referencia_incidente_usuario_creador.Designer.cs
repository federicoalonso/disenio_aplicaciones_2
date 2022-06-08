﻿// <auto-generated />
using Incidentes.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Incidentes.Datos.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20211006201746_referencia_incidente_usuario_creador")]
    partial class referencia_incidente_usuario_creador
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Incidentes.Dominio.Incidente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DesarrolladorId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstadoIncidente")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProyectoId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ProyectoId");

                    b.ToTable("Incidentes");
                });

            modelBuilder.Entity("Incidentes.Dominio.Proyecto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("Incidentes.Dominio.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contrasenia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUsuario")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RolUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NombreUsuario")
                        .IsUnique()
                        .HasFilter("[NombreUsuario] IS NOT NULL");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ProyectoUsuario", b =>
                {
                    b.Property<int>("AsignadosId")
                        .HasColumnType("int");

                    b.Property<int>("proyectosId")
                        .HasColumnType("int");

                    b.HasKey("AsignadosId", "proyectosId");

                    b.HasIndex("proyectosId");

                    b.ToTable("ProyectoUsuario");
                });

            modelBuilder.Entity("Incidentes.Dominio.Incidente", b =>
                {
                    b.HasOne("Incidentes.Dominio.Proyecto", null)
                        .WithMany("Incidentes")
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProyectoUsuario", b =>
                {
                    b.HasOne("Incidentes.Dominio.Usuario", null)
                        .WithMany()
                        .HasForeignKey("AsignadosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incidentes.Dominio.Proyecto", null)
                        .WithMany()
                        .HasForeignKey("proyectosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Incidentes.Dominio.Proyecto", b =>
                {
                    b.Navigation("Incidentes");
                });
#pragma warning restore 612, 618
        }
    }
}
