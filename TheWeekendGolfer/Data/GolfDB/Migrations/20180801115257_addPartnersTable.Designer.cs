﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheWeekendGolfer.Web.Data;

namespace TheWeekendGolfer.Data.GolfDb.Migrations
{
    [DbContext(typeof(GolfDbContext))]
    [Migration("20180801115257_addPartnersTable")]
    partial class addPartnersTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TheWeekendGolfer.Models.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Holes");

                    b.Property<string>("Location");

                    b.Property<string>("Name");

                    b.Property<int>("Par");

                    b.Property<decimal>("ScratchRating");

                    b.Property<decimal>("Slope");

                    b.Property<string>("TeeName");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TheWeekendGolfer.Models.GolfRound", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CourseId");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.HasKey("Id");

                    b.ToTable("GolfRounds");
                });

            modelBuilder.Entity("TheWeekendGolfer.Models.Handicap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("PlayerId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("Handicaps");
                });

            modelBuilder.Entity("TheWeekendGolfer.Models.Partner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PartnerId");

                    b.Property<Guid>("PlayerId");

                    b.HasKey("Id");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("TheWeekendGolfer.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<decimal?>("Handicap");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("Modified");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TheWeekendGolfer.Models.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("GolfRoundId");

                    b.Property<Guid>("PlayerId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.ToTable("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
