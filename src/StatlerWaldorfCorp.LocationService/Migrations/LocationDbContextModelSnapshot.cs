﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StatlerWaldorfCorp.LocationService.Persistence;

namespace StatlerWaldorfCorp.LocationService.Migrations
{
    [DbContext(typeof(LocationDbContext))]
    partial class LocationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("StatlerWaldorfCorp.LocationService.Models.LocationRecord", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Altitude");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<Guid>("MemberID");

                    b.Property<long>("Timestamp");

                    b.HasKey("ID");

                    b.ToTable("LocationRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
