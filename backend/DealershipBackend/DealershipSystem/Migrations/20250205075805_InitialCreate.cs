using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DealershipSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameJapanese = table.Column<string>(type: "text", nullable: false),
                    NameEnglish = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CarMakers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BrandEnglish = table.Column<string>(type: "text", nullable: false),
                    BrandJapanese = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMakers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColorNameEnglish = table.Column<string>(type: "text", nullable: false),
                    ColorNameJapanese = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DrivetrainTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivetrainTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameJapanese = table.Column<string>(type: "text", nullable: false),
                    NameEnglish = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prefectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    NameJP = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefectures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransmissionTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransmissionTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    NameKanji = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    PreferredLanguage = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MakerID = table.Column<int>(type: "integer", nullable: false),
                    ModelNameJapanese = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModelNameEnglish = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModelCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ManufacturingStartYear = table.Column<int>(type: "integer", nullable: false),
                    ManufacturingEndYear = table.Column<int>(type: "integer", nullable: false),
                    PassengerCount = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Mass = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CarModels_CarMakers_MakerID",
                        column: x => x.MakerID,
                        principalTable: "CarMakers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostalCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PrefectureId = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    CityRomanized = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    StreetRomanized = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Prefectures_PrefectureId",
                        column: x => x.PrefectureId,
                        principalTable: "Prefectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EngineSizeModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelID = table.Column<int>(type: "integer", nullable: false),
                    EngineSize = table.Column<int>(type: "integer", nullable: false),
                    FuelTypeID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineSizeModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EngineSizeModels_CarModels_ModelID",
                        column: x => x.ModelID,
                        principalTable: "CarModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngineSizeModels_FuelTypes_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    MaxCapacity = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BrandID = table.Column<int>(type: "integer", nullable: false),
                    ModelID = table.Column<int>(type: "integer", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    BodyTypeID = table.Column<int>(type: "integer", nullable: false),
                    LocationID = table.Column<int>(type: "integer", nullable: false),
                    EngineSizeID = table.Column<int>(type: "integer", nullable: false),
                    LicensePlateNumber = table.Column<string>(type: "text", nullable: true),
                    RepairHistory = table.Column<bool>(type: "boolean", nullable: false),
                    FuelTypeID = table.Column<int>(type: "integer", nullable: false),
                    DriveTrainID = table.Column<int>(type: "integer", nullable: false),
                    MOTExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransmissionTypeID = table.Column<int>(type: "integer", nullable: false),
                    VINNum = table.Column<string>(type: "text", nullable: false),
                    ColorID = table.Column<int>(type: "integer", nullable: false),
                    IsSmoking = table.Column<bool>(type: "boolean", nullable: false),
                    IsInTransfer = table.Column<bool>(type: "boolean", nullable: false),
                    CarModelID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cars_BodyTypes_BodyTypeID",
                        column: x => x.BodyTypeID,
                        principalTable: "BodyTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_CarMakers_BrandID",
                        column: x => x.BrandID,
                        principalTable: "CarMakers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_CarModels_CarModelID",
                        column: x => x.CarModelID,
                        principalTable: "CarModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_DrivetrainTypes_DriveTrainID",
                        column: x => x.DriveTrainID,
                        principalTable: "DrivetrainTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_EngineSizeModels_EngineSizeID",
                        column: x => x.EngineSizeID,
                        principalTable: "EngineSizeModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cars_FuelTypes_FuelTypeID",
                        column: x => x.FuelTypeID,
                        principalTable: "FuelTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_TransmissionTypes_TransmissionTypeID",
                        column: x => x.TransmissionTypeID,
                        principalTable: "TransmissionTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarExtras",
                columns: table => new
                {
                    CarExtraID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarID = table.Column<int>(type: "integer", nullable: false),
                    ExtraID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarExtras", x => x.CarExtraID);
                    table.ForeignKey(
                        name: "FK_CarExtras_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarExtras_Cars_ExtraID",
                        column: x => x.ExtraID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarID = table.Column<int>(type: "integer", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BodyTypes",
                columns: new[] { "ID", "NameEnglish", "NameJapanese" },
                values: new object[,]
                {
                    { 1, "Kei Car", "軽自動車" },
                    { 2, "Compact Car", "コンパクトカー" },
                    { 3, "Minivan/One-Box", "ミニバン/ワンボックス" },
                    { 4, "Sedan", "セダン" },
                    { 5, "Coupe", "クーペ" },
                    { 6, "Station Wagon", "ステーションワゴン" },
                    { 7, "SUV/Crossover", "SUV/クロカン" },
                    { 8, "Truck/Van", "トラック/バン" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "ID", "ColorNameEnglish", "ColorNameJapanese" },
                values: new object[,]
                {
                    { 1, "White", "白系" },
                    { 2, "Black", "黒系" },
                    { 3, "Red", "赤系" },
                    { 4, "Blue", "青系" },
                    { 5, "Silver", "シルバー系" },
                    { 6, "Gray", "グレー系" },
                    { 7, "Gold", "金系" },
                    { 8, "Beige", "ベージュ系" },
                    { 9, "Brown", "ブラウン系" },
                    { 10, "Orange", "オレンジ系" },
                    { 11, "Pink", "ピンク系" },
                    { 12, "Purple", "紫系" },
                    { 13, "Yellow", "黄系" },
                    { 14, "Green", "緑系" },
                    { 15, "Other", "その他" }
                });

            migrationBuilder.InsertData(
                table: "DrivetrainTypes",
                columns: new[] { "ID", "Type" },
                values: new object[,]
                {
                    { 1, "FWD" },
                    { 2, "RWD" },
                    { 3, "AWD" },
                    { 4, "4WD" }
                });

            migrationBuilder.InsertData(
                table: "FuelTypes",
                columns: new[] { "ID", "NameEnglish", "NameJapanese" },
                values: new object[,]
                {
                    { 1, "Gasoline", "ガソリン" },
                    { 2, "Diesel", "ディーゼル" },
                    { 3, "Hybrid", "ハイブリッド" },
                    { 4, "Electric", "電気" },
                    { 5, "LPG", "LPG" }
                });

            migrationBuilder.InsertData(
                table: "Prefectures",
                columns: new[] { "Id", "Name", "NameJP" },
                values: new object[,]
                {
                    { 1, "Hokkaido", "北海道" },
                    { 2, "Aomori", "青森県" },
                    { 3, "Iwate", "岩手県" },
                    { 4, "Miyagi", "宮城県" },
                    { 5, "Akita", "秋田県" },
                    { 6, "Yamagata", "山形県" },
                    { 7, "Fukushima", "福島県" },
                    { 8, "Ibaraki", "茨城県" },
                    { 9, "Tochigi", "栃木県" },
                    { 10, "Gunma", "群馬県" },
                    { 11, "Saitama", "埼玉県" },
                    { 12, "Chiba", "千葉県" },
                    { 13, "Tokyo", "東京都" },
                    { 14, "Kanagawa", "神奈川県" },
                    { 15, "Niigata", "新潟県" },
                    { 16, "Toyama", "富山県" },
                    { 17, "Ishikawa", "石川県" },
                    { 18, "Fukui", "福井県" },
                    { 19, "Yamanashi", "山梨県" },
                    { 20, "Nagano", "長野県" },
                    { 21, "Gifu", "岐阜県" },
                    { 22, "Shizuoka", "静岡県" },
                    { 23, "Aichi", "愛知県" },
                    { 24, "Mie", "三重県" },
                    { 25, "Shiga", "滋賀県" },
                    { 26, "Kyoto", "京都府" },
                    { 27, "Osaka", "大阪府" },
                    { 28, "Hyogo", "兵庫県" },
                    { 29, "Nara", "奈良県" },
                    { 30, "Wakayama", "和歌山県" },
                    { 31, "Tottori", "鳥取県" },
                    { 32, "Shimane", "島根県" },
                    { 33, "Okayama", "岡山県" },
                    { 34, "Hiroshima", "広島県" },
                    { 35, "Yamaguchi", "山口県" },
                    { 36, "Tokushima", "徳島県" },
                    { 37, "Kagawa", "香川県" },
                    { 38, "Ehime", "愛媛県" },
                    { 39, "Kochi", "高知県" },
                    { 40, "Fukuoka", "福岡県" },
                    { 41, "Saga", "佐賀県" },
                    { 42, "Nagasaki", "長崎県" },
                    { 43, "Kumamoto", "熊本県" },
                    { 44, "Oita", "大分県" },
                    { 45, "Miyazaki", "宮崎県" },
                    { 46, "Kagoshima", "鹿児島県" },
                    { 47, "Okinawa", "沖縄県" }
                });

            migrationBuilder.InsertData(
                table: "TransmissionTypes",
                columns: new[] { "ID", "Type" },
                values: new object[,]
                {
                    { 1, "MT" },
                    { 2, "AT" },
                    { 3, "CVT" },
                    { 4, "AMT" },
                    { 5, "DCT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PrefectureId",
                table: "Addresses",
                column: "PrefectureId");

            migrationBuilder.CreateIndex(
                name: "IX_CarExtras_CarID",
                table: "CarExtras",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_CarExtras_ExtraID",
                table: "CarExtras",
                column: "ExtraID");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_MakerID",
                table: "CarModels",
                column: "MakerID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BodyTypeID",
                table: "Cars",
                column: "BodyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandID",
                table: "Cars",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarModelID",
                table: "Cars",
                column: "CarModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ColorID",
                table: "Cars",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriveTrainID",
                table: "Cars",
                column: "DriveTrainID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_EngineSizeID",
                table: "Cars",
                column: "EngineSizeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_FuelTypeID",
                table: "Cars",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_LocationID",
                table: "Cars",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TransmissionTypeID",
                table: "Cars",
                column: "TransmissionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EngineSizeModels_FuelTypeID",
                table: "EngineSizeModels",
                column: "FuelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EngineSizeModels_ModelID",
                table: "EngineSizeModels",
                column: "ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarID",
                table: "Images",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AddressId",
                table: "Locations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefectures_Name",
                table: "Prefectures",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prefectures_NameJP",
                table: "Prefectures",
                column: "NameJP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarExtras");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "BodyTypes");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "DrivetrainTypes");

            migrationBuilder.DropTable(
                name: "EngineSizeModels");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "TransmissionTypes");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "CarMakers");

            migrationBuilder.DropTable(
                name: "Prefectures");
        }
    }
}
