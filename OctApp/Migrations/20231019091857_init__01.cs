using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class init__01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 10, 18, 57, 354, DateTimeKind.Local).AddTicks(6170), new DateTime(2023, 10, 19, 10, 18, 57, 354, DateTimeKind.Local).AddTicks(6200) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 10, 18, 57, 354, DateTimeKind.Local).AddTicks(6210), new DateTime(2023, 10, 19, 10, 18, 57, 354, DateTimeKind.Local).AddTicks(6210) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 8, 29, 46, 574, DateTimeKind.Local).AddTicks(4500), new DateTime(2023, 10, 19, 8, 29, 46, 574, DateTimeKind.Local).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 8, 29, 46, 574, DateTimeKind.Local).AddTicks(4530), new DateTime(2023, 10, 19, 8, 29, 46, 574, DateTimeKind.Local).AddTicks(4530) });
        }
    }
}
