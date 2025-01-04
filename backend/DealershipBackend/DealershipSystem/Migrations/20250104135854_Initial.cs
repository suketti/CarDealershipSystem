using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DealershipSystem.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostalCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PrefectureId",
                table: "Addresses",
                column: "PrefectureId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Prefectures");
        }
    }
}
