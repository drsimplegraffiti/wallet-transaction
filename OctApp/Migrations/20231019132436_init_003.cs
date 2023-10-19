using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class init_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 24, 36, 633, DateTimeKind.Local).AddTicks(4570), new DateTime(2023, 10, 19, 14, 24, 36, 633, DateTimeKind.Local).AddTicks(4600) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 24, 36, 633, DateTimeKind.Local).AddTicks(4610), new DateTime(2023, 10, 19, 14, 24, 36, 633, DateTimeKind.Local).AddTicks(4610) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 22, 1, 904, DateTimeKind.Local).AddTicks(3150), new DateTime(2023, 10, 19, 14, 22, 1, 904, DateTimeKind.Local).AddTicks(3190) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 22, 1, 904, DateTimeKind.Local).AddTicks(3190), new DateTime(2023, 10, 19, 14, 22, 1, 904, DateTimeKind.Local).AddTicks(3200) });
        }
    }
}
