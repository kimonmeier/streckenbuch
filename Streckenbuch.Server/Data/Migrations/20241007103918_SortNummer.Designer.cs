﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Streckenbuch.Server.Data;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241007103918_SortNummer")]
    partial class SortNummer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Kommentar")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<short>("Typ")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Betriebspunkt", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Betriebspunkte.BetriebspunktStreckenZuordnung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BetriebspunktId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortNummer")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StreckenKonfigurationId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BetriebspunktId");

                    b.HasIndex("StreckenKonfigurationId", "BetriebspunktId")
                        .IsUnique();

                    b.HasIndex("StreckenKonfigurationId", "SortNummer")
                        .IsUnique();

                    b.ToTable("BetriebspunktStreckenZuordnung", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Signale.Signal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BetriebspunktId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Seite")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Typ")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BetriebspunktId");

                    b.ToTable("Signal", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Signale.SignalStreckenZuordnung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("GueltigAb")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("GueltigBis")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SignalId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StreckeId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SignalId");

                    b.HasIndex("StreckeId", "SignalId");

                    b.ToTable("SignalStreckenZuordnung", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Strecken.Strecke", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("StreckenNummer")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StreckenNummer")
                        .IsUnique();

                    b.ToTable("Strecke", (string)null);
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Strecken.StreckenKonfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BisBetriebspunktId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StreckeId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VonBetriebspunktId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BisBetriebspunktId");

                    b.HasIndex("VonBetriebspunktId");

                    b.HasIndex("StreckeId", "VonBetriebspunktId", "BisBetriebspunktId")
                        .IsUnique();

                    b.ToTable("StreckenKonfiguration", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streckenbuch.Server.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", b =>
                {
                    b.OwnsOne("NetTopologySuite.Geometries.Coordinate", "Location", b1 =>
                        {
                            b1.Property<Guid>("BetriebspunktId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("X")
                                .HasColumnType("REAL");

                            b1.Property<double>("Y")
                                .HasColumnType("REAL");

                            b1.HasKey("BetriebspunktId");

                            b1.ToTable("Betriebspunkt");

                            b1.WithOwner()
                                .HasForeignKey("BetriebspunktId");
                        });

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Betriebspunkte.BetriebspunktStreckenZuordnung", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", "Betriebspunkt")
                        .WithMany()
                        .HasForeignKey("BetriebspunktId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streckenbuch.Server.Data.Entities.Strecken.StreckenKonfiguration", "StreckenKonfiguration")
                        .WithMany()
                        .HasForeignKey("StreckenKonfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Betriebspunkt");

                    b.Navigation("StreckenKonfiguration");
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Signale.Signal", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", "Betriebspunkt")
                        .WithMany()
                        .HasForeignKey("BetriebspunktId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("NetTopologySuite.Geometries.Coordinate", "Location", b1 =>
                        {
                            b1.Property<Guid>("SignalId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("X")
                                .HasColumnType("REAL");

                            b1.Property<double>("Y")
                                .HasColumnType("REAL");

                            b1.HasKey("SignalId");

                            b1.ToTable("Signal");

                            b1.WithOwner()
                                .HasForeignKey("SignalId");
                        });

                    b.Navigation("Betriebspunkt");

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Signale.SignalStreckenZuordnung", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.Signale.Signal", "Signal")
                        .WithMany()
                        .HasForeignKey("SignalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streckenbuch.Server.Data.Entities.Betriebspunkte.BetriebspunktStreckenZuordnung", "Strecke")
                        .WithMany()
                        .HasForeignKey("StreckeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Signal");

                    b.Navigation("Strecke");
                });

            modelBuilder.Entity("Streckenbuch.Server.Data.Entities.Strecken.StreckenKonfiguration", b =>
                {
                    b.HasOne("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", "BisBetriebspunkt")
                        .WithMany()
                        .HasForeignKey("BisBetriebspunktId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streckenbuch.Server.Data.Entities.Strecken.Strecke", "Strecke")
                        .WithMany()
                        .HasForeignKey("StreckeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Streckenbuch.Server.Data.Entities.Betriebspunkte.Betriebspunkt", "VonBetriebspunkt")
                        .WithMany()
                        .HasForeignKey("VonBetriebspunktId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BisBetriebspunkt");

                    b.Navigation("Strecke");

                    b.Navigation("VonBetriebspunkt");
                });
#pragma warning restore 612, 618
        }
    }
}
