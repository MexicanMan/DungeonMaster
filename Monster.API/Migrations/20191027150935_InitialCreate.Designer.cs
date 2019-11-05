﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monster.API.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Monster.API.Migrations
{
    [DbContext(typeof(MonsterContext))]
    [Migration("20191027150935_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Monster.API.Models.MonsterModel", b =>
                {
                    b.Property<int>("MonsterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CurrentHP")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<int>("MaxHP")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.HasKey("MonsterId");

                    b.HasIndex("MonsterId")
                        .IsUnique();

                    b.ToTable("Monsters");
                });
#pragma warning restore 612, 618
        }
    }
}
