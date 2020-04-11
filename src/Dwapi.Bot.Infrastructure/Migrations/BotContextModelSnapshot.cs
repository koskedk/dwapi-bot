﻿// <auto-generated />
using System;
using Dwapi.Bot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    [DbContext(typeof(BotContext))]
    partial class BotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Configs.MatchConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MatchStatus")
                        .HasColumnType("int");

                    b.Property<double?>("MaxThreshold")
                        .HasColumnType("float");

                    b.Property<double?>("MinThreshold")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("MatchConfigs");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Indices.SubjectIndex", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FacilityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FacilityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MpiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PatientID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientPk")
                        .HasColumnType("int");

                    b.Property<int>("RowId")
                        .HasColumnType("int");

                    b.Property<string>("Serial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SiteCode")
                        .HasColumnType("int");

                    b.Property<string>("dmFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dmLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dmMiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dmPKValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dmPKValueDoB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxMiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxPKValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxPKValueDoB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxdmPKValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sxdmPKValueDoB")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubjectIndices");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Indices.SubjectIndexScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Field")
                        .HasColumnType("int");

                    b.Property<Guid>("OtherSubjectIndexId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ScanLevel")
                        .HasColumnType("int");

                    b.Property<string>("ScanLevelCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("SubjectIndexId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectIndexId");

                    b.ToTable("SubjectIndexScores");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Indices.SubjectIndexStage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StatusInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SubjectIndexId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SubjectIndexId");

                    b.ToTable("SubjectIndexStages");
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Indices.SubjectIndexScore", b =>
                {
                    b.HasOne("Dwapi.Bot.Core.Domain.Indices.SubjectIndex", null)
                        .WithMany("IndexScores")
                        .HasForeignKey("SubjectIndexId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dwapi.Bot.Core.Domain.Indices.SubjectIndexStage", b =>
                {
                    b.HasOne("Dwapi.Bot.Core.Domain.Indices.SubjectIndex", null)
                        .WithMany("IndexStages")
                        .HasForeignKey("SubjectIndexId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
