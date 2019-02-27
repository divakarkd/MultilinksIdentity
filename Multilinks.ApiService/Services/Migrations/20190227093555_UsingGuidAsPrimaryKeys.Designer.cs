﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Multilinks.ApiService.Services;

namespace Multilinks.ApiService.Services.Migrations
{
    [DbContext(typeof(ApiServiceDbContext))]
    [Migration("20190227093555_UsingGuidAsPrimaryKeys")]
    partial class UsingGuidAsPrimaryKeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Multilinks.ApiService.Entities.EndpointEntity", b =>
                {
                    b.Property<Guid>("EndpointId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId");

                    b.Property<string>("ClientType");

                    b.Property<Guid>("CreatorId");

                    b.Property<string>("CreatorName");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("EndpointId");

                    b.ToTable("Endpoints");
                });

            modelBuilder.Entity("Multilinks.ApiService.Entities.EndpointLinkEntity", b =>
                {
                    b.Property<Guid>("LinkId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AssociatedEndpointId");

                    b.Property<Guid>("SourceEndpointId");

                    b.Property<string>("Status");

                    b.HasKey("LinkId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Multilinks.ApiService.Entities.HubConnectionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionId");

                    b.Property<Guid>("EndpointId");

                    b.HasKey("Id");

                    b.ToTable("HubConnections");
                });
#pragma warning restore 612, 618
        }
    }
}
