﻿// <auto-generated />
using System;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(PgSqlDbContext))]
    partial class PgSqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Domain.Entities.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Domain.Entities.LiveStream", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GuestsCount")
                        .HasColumnType("integer");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<string>("MainQuestion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WorldId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StreamerId");

                    b.HasIndex("WorldId");

                    b.ToTable("LiveStreams");
                });

            modelBuilder.Entity("Domain.Entities.Node", b =>
                {
                    b.Property<Guid>("LinkId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LiveStreamId")
                        .HasColumnType("uuid");

                    b.HasKey("LinkId", "LiveStreamId");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("Domain.Entities.OtpCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.HasKey("Id");

                    b.ToTable("OtpCodes");
                });

            modelBuilder.Entity("Domain.Entities.Participants", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LiveStreamId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastStatusChangeDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Permission")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("TotalDuration")
                        .HasColumnType("interval");

                    b.HasKey("UserId", "LiveStreamId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRegisterCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLoginDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpireTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("4f590a70-94ce-4d15-9494-5248280c2ce3"),
                            Avatar = "im am here",
                            CreateDateTime = new DateTime(2022, 9, 5, 9, 0, 11, 96, DateTimeKind.Utc).AddTicks(1050),
                            FirstName = "mediasoup",
                            IsAdmin = true,
                            IsRegisterCompleted = true,
                            LastLoginDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "server",
                            PhoneNumber = "09009009000",
                            RefreshToken = "qkjbcoi238yehasd",
                            RefreshTokenExpireTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "Admin_mediasoup_server"
                        });
                });

            modelBuilder.Entity("Domain.Entities.World", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("World");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "استارت آپ و کسب و کار"
                        },
                        new
                        {
                            Id = new Guid("f68eb329-64f6-425c-9052-f0cf42117524"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "آموزشی"
                        },
                        new
                        {
                            Id = new Guid("6018a1df-ad09-4005-8c36-29db689fc7e6"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "اقتصاد، بورس، مالی، سرمایه گذاری"
                        },
                        new
                        {
                            Id = new Guid("e8bdbbdf-4624-4501-9c39-505c60316c1b"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "حقوقی، قضایی، وکالت"
                        },
                        new
                        {
                            Id = new Guid("17d0f3ef-6ff3-4868-b924-c13c5c45ab49"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "ورزشی، مد، زیبایی و تغذیه"
                        },
                        new
                        {
                            Id = new Guid("0f3ac0a6-655e-4498-8590-eaabd5788e9a"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "مشاوره و روانشناسی"
                        },
                        new
                        {
                            Id = new Guid("2cb6f432-2472-4681-8f22-4c4a94853f0d"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "بیمه، مالیات، شرکت"
                        },
                        new
                        {
                            Id = new Guid("7638e5c6-cba1-4071-9a0b-ecff4d9c645d"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "سلامتی و پزشکی"
                        },
                        new
                        {
                            Id = new Guid("c0f3220a-d688-4535-ae72-feabe2e61f2c"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "سرمایه انسانی و توسعه فردی"
                        },
                        new
                        {
                            Id = new Guid("c5df6845-f8c0-4062-9ebd-7079a5a5f615"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "مهاجرت و ادامه ی تحصیل"
                        },
                        new
                        {
                            Id = new Guid("8f00ea20-aec9-4051-ba2c-9397ee406e63"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "نظلم وظیفه و اداری"
                        },
                        new
                        {
                            Id = new Guid("e1c1ed84-b379-4c24-9121-c1c6263491b3"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "فلسفه، دین، مذهب"
                        },
                        new
                        {
                            Id = new Guid("ee19c0cc-ed03-4c6b-84cd-a1e5f31ecada"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "خرید و فروش"
                        },
                        new
                        {
                            Id = new Guid("07b2f146-b624-4c8e-bdf0-e5172c5e9f1c"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "فرهنگ، هنر، معماری"
                        },
                        new
                        {
                            Id = new Guid("edf6a3f8-c2f1-454d-85e9-11eab8fad28f"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "تکنولوژی و مهندسی"
                        },
                        new
                        {
                            Id = new Guid("4bf0d2a9-b62b-4f58-a18b-a1eef5c3b9c7"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "تفریح، سرگرمی، سفر"
                        },
                        new
                        {
                            Id = new Guid("10fd49fd-2187-406a-830c-b367035cd448"),
                            CreateDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "سیاست"
                        });
                });

            modelBuilder.Entity("Domain.Entities.LiveStream", b =>
                {
                    b.HasOne("Domain.Entities.User", "Streamer")
                        .WithMany("LiveStreams")
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.World", "World")
                        .WithMany("LiveStreams")
                        .HasForeignKey("WorldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");

                    b.Navigation("World");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("LiveStreams");
                });

            modelBuilder.Entity("Domain.Entities.World", b =>
                {
                    b.Navigation("LiveStreams");
                });
#pragma warning restore 612, 618
        }
    }
}