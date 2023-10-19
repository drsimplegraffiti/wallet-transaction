using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 37, 3, 303, DateTimeKind.Local).AddTicks(6850), new DateTime(2023, 10, 19, 14, 37, 3, 303, DateTimeKind.Local).AddTicks(6870) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 37, 3, 303, DateTimeKind.Local).AddTicks(6880), new DateTime(2023, 10, 19, 14, 37, 3, 303, DateTimeKind.Local).AddTicks(6880) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
