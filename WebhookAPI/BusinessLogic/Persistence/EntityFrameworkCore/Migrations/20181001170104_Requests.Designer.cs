﻿// <auto-generated />
using System;
using Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gitloy.Services.WebhookAPI.Migrations
{
    [DbContext(typeof(WebhookApiContext))]
    [Migration("20181001170104_Requests")]
    partial class Requests
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.FtpNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hostname")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("Port");

                    b.Property<string>("RootDirectory");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("FtpNodes");
                });

            modelBuilder.Entity("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.GitRepo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Branch");

                    b.Property<int>("FtpNodeId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("FtpNodeId")
                        .IsUnique();

                    b.ToTable("GitRepos");
                });

            modelBuilder.Entity("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GitId");

                    b.Property<string>("ResultDetails");

                    b.Property<string>("ResultMessage");

                    b.Property<int>("ResultStatus");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("GitId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.GitRepo", b =>
                {
                    b.HasOne("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.FtpNode", "FtpNode")
                        .WithOne("GitRepo")
                        .HasForeignKey("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.GitRepo", "FtpNodeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.Request", b =>
                {
                    b.HasOne("Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model.GitRepo", "Git")
                        .WithMany()
                        .HasForeignKey("GitId");
                });
#pragma warning restore 612, 618
        }
    }
}
