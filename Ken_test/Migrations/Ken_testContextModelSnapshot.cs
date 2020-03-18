﻿// <auto-generated />
using System;
using Ken_test.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ken_test.Migrations
{
    [DbContext(typeof(Ken_testContext))]
    partial class Ken_testContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Ken_test.Models.MessageLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("DateTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("MsgContext")
                        .HasMaxLength(500);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("message_log");
                });

            modelBuilder.Entity("Ken_test.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("DateTime");

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("DateTime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("NickName")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasMaxLength(50);

                    b.Property<string>("RealName")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("user_info");
                });

            modelBuilder.Entity("Ken_test.Models.MessageLog", b =>
                {
                    b.HasOne("Ken_test.Models.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}