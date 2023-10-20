using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class umper_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7400), new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7440), new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7440) });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "BankCode", "BankId", "BankName", "CreatedAt", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "044", "044", "Access Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7490), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7490) },
                    { 2, "023", "023", "Citibank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7500), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7500) },
                    { 3, "063", "063", "Diamond Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7500), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7500) },
                    { 4, "0", "0", "Dynamic Standard Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7510), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7510) },
                    { 5, "050", "050", "Ecobank Nigeria", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7510), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7510) },
                    { 6, "070", "070", "Fidelity Bank Nigeria", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7510), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7520) },
                    { 7, "011", "011", "First Bank of Nigeria", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7520), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7520) },
                    { 8, "214", "214", "First City Monument Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7520), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7520) },
                    { 9, "058", "058", "Guaranty Trust Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530) },
                    { 10, "030", "030", "Heritage Bank Plc", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530) },
                    { 11, "301", "301", "Jaiz Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7530) },
                    { 12, "082", "082", "Keystone Bank Limited", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7540), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7540) },
                    { 13, "101", "101", "Providus Bank Plc", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7540), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7540) },
                    { 14, "076", "076", "Polaris Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7540), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7550) },
                    { 15, "221", "221", "Stanbic IBTC Bank Nigeria Limited", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7550), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7550) },
                    { 16, "068", "068", "Standard Chartered Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7550), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7550) },
                    { 17, "232", "232", "Sterling Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560) },
                    { 18, "100", "100", "Suntrust Bank Nigeria Limited", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560) },
                    { 19, "032", "032", "Union Bank of Nigeria", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7560) },
                    { 20, "033", "033", "United Bank for Africa", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570) },
                    { 21, "215", "215", "Unity Bank Plc", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570) },
                    { 22, "035", "035", "Wema Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7570) },
                    { 23, "057", "057", "Zenith Bank", new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7580), false, new DateTime(2023, 10, 19, 17, 38, 5, 508, DateTimeKind.Local).AddTicks(7580) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 47, 3, 937, DateTimeKind.Local).AddTicks(2740), new DateTime(2023, 10, 19, 14, 47, 3, 937, DateTimeKind.Local).AddTicks(2770) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 47, 3, 937, DateTimeKind.Local).AddTicks(2780), new DateTime(2023, 10, 19, 14, 47, 3, 937, DateTimeKind.Local).AddTicks(2780) });
        }
    }
}
