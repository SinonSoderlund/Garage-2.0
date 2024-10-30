using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class seedvehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "Id", "ArriveTime", "Brand", "Color", "Model", "RegNr", "VehicleType", "Wheels" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ferrari", "Red", "F40", "abc123", 0, 4L },
                    { 2, new DateTime(2024, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota", "Yellow", "Supra", "def456", 0, 4L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
