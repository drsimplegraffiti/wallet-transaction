using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OctApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_0023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 46, 1, 400, DateTimeKind.Local).AddTicks(2730), new DateTime(2023, 10, 19, 14, 46, 1, 400, DateTimeKind.Local).AddTicks(2750) });

            migrationBuilder.UpdateData(
                table: "AppEnvironments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2023, 10, 19, 14, 46, 1, 400, DateTimeKind.Local).AddTicks(2760), new DateTime(2023, 10, 19, 14, 46, 1, 400, DateTimeKind.Local).AddTicks(2760) });
        }
    }
}
