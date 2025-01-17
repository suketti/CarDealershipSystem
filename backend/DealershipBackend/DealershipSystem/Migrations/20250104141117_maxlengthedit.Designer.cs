﻿// <auto-generated />
using DealershipSystem.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DealershipSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250104141117_maxlengthedit")]
    partial class maxlengthedit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DealershipSystem.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CityRomanized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<int>("PrefectureId")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StreetRomanized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PrefectureId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DealershipSystem.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("ID");

                    b.HasIndex("AddressId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("DealershipSystem.Models.Prefecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("NameJP")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("NameJP")
                        .IsUnique();

                    b.ToTable("Prefectures");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Hokkaido",
                            NameJP = "北海道"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Aomori",
                            NameJP = "青森県"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Iwate",
                            NameJP = "岩手県"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Miyagi",
                            NameJP = "宮城県"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Akita",
                            NameJP = "秋田県"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Yamagata",
                            NameJP = "山形県"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Fukushima",
                            NameJP = "福島県"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Ibaraki",
                            NameJP = "茨城県"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Tochigi",
                            NameJP = "栃木県"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Gunma",
                            NameJP = "群馬県"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Saitama",
                            NameJP = "埼玉県"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Chiba",
                            NameJP = "千葉県"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Tokyo",
                            NameJP = "東京都"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Kanagawa",
                            NameJP = "神奈川県"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Niigata",
                            NameJP = "新潟県"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Toyama",
                            NameJP = "富山県"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Ishikawa",
                            NameJP = "石川県"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Fukui",
                            NameJP = "福井県"
                        },
                        new
                        {
                            Id = 19,
                            Name = "Yamanashi",
                            NameJP = "山梨県"
                        },
                        new
                        {
                            Id = 20,
                            Name = "Nagano",
                            NameJP = "長野県"
                        },
                        new
                        {
                            Id = 21,
                            Name = "Gifu",
                            NameJP = "岐阜県"
                        },
                        new
                        {
                            Id = 22,
                            Name = "Shizuoka",
                            NameJP = "静岡県"
                        },
                        new
                        {
                            Id = 23,
                            Name = "Aichi",
                            NameJP = "愛知県"
                        },
                        new
                        {
                            Id = 24,
                            Name = "Mie",
                            NameJP = "三重県"
                        },
                        new
                        {
                            Id = 25,
                            Name = "Shiga",
                            NameJP = "滋賀県"
                        },
                        new
                        {
                            Id = 26,
                            Name = "Kyoto",
                            NameJP = "京都府"
                        },
                        new
                        {
                            Id = 27,
                            Name = "Osaka",
                            NameJP = "大阪府"
                        },
                        new
                        {
                            Id = 28,
                            Name = "Hyogo",
                            NameJP = "兵庫県"
                        },
                        new
                        {
                            Id = 29,
                            Name = "Nara",
                            NameJP = "奈良県"
                        },
                        new
                        {
                            Id = 30,
                            Name = "Wakayama",
                            NameJP = "和歌山県"
                        },
                        new
                        {
                            Id = 31,
                            Name = "Tottori",
                            NameJP = "鳥取県"
                        },
                        new
                        {
                            Id = 32,
                            Name = "Shimane",
                            NameJP = "島根県"
                        },
                        new
                        {
                            Id = 33,
                            Name = "Okayama",
                            NameJP = "岡山県"
                        },
                        new
                        {
                            Id = 34,
                            Name = "Hiroshima",
                            NameJP = "広島県"
                        },
                        new
                        {
                            Id = 35,
                            Name = "Yamaguchi",
                            NameJP = "山口県"
                        },
                        new
                        {
                            Id = 36,
                            Name = "Tokushima",
                            NameJP = "徳島県"
                        },
                        new
                        {
                            Id = 37,
                            Name = "Kagawa",
                            NameJP = "香川県"
                        },
                        new
                        {
                            Id = 38,
                            Name = "Ehime",
                            NameJP = "愛媛県"
                        },
                        new
                        {
                            Id = 39,
                            Name = "Kochi",
                            NameJP = "高知県"
                        },
                        new
                        {
                            Id = 40,
                            Name = "Fukuoka",
                            NameJP = "福岡県"
                        },
                        new
                        {
                            Id = 41,
                            Name = "Saga",
                            NameJP = "佐賀県"
                        },
                        new
                        {
                            Id = 42,
                            Name = "Nagasaki",
                            NameJP = "長崎県"
                        },
                        new
                        {
                            Id = 43,
                            Name = "Kumamoto",
                            NameJP = "熊本県"
                        },
                        new
                        {
                            Id = 44,
                            Name = "Oita",
                            NameJP = "大分県"
                        },
                        new
                        {
                            Id = 45,
                            Name = "Miyazaki",
                            NameJP = "宮崎県"
                        },
                        new
                        {
                            Id = 46,
                            Name = "Kagoshima",
                            NameJP = "鹿児島県"
                        },
                        new
                        {
                            Id = 47,
                            Name = "Okinawa",
                            NameJP = "沖縄県"
                        });
                });

            modelBuilder.Entity("DealershipSystem.Models.Address", b =>
                {
                    b.HasOne("DealershipSystem.Models.Prefecture", "Prefecture")
                        .WithMany()
                        .HasForeignKey("PrefectureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prefecture");
                });

            modelBuilder.Entity("DealershipSystem.Models.Location", b =>
                {
                    b.HasOne("DealershipSystem.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}
