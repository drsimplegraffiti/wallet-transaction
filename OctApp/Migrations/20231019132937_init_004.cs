using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class init_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 29, 37, 264, DateTimeKind.Local).AddTicks(4740), new DateTime(2023, 10, 19, 14, 29, 37, 264, DateTimeKind.Local).AddTicks(4790) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 29, 37, 264, DateTimeKind.Local).AddTicks(4790), new DateTime(2023, 10, 19, 14, 29, 37, 264, DateTimeKind.Local).AddTicks(4790) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
