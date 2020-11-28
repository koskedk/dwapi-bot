﻿// <auto-generated />
using System;
using Dwapi.Bot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dwapi.Bot.Infrastructure.Migrations.BotCleaner
{
    [DbContext(typeof(BotCleanerContext))]
    partial class BotCleanerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Catalogs.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Docket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FacilityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Store")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Catalogs.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Extract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PatientPk")
                        .HasColumnType("int");

                    b.Property<Guid?>("PreferredPatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Catalogs.SubjectExtract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CandidatePatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Extract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ExtractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PreferredExtractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Extracts");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Catalogs.Subject", b =>
                {
                    b.HasOne("Dwapi.Bot.Core.Domain.Catalogs.Site", null)
                        .WithMany("Subjects")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Catalogs.SubjectExtract", b =>
                {
                    b.HasOne("Dwapi.Bot.Core.Domain.Catalogs.Site", null)
                        .WithMany("SubjectExtracts")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
