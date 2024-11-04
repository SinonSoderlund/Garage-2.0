using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddSpot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "Spots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spots_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Spots",
                columns: new[] { "Id", "VehicleId" },
                values: new object[,]
                {
                    { 1, null },
                    { 2, null },
                    { 3, null },
                    { 4, null },
                    { 5, null },
                    { 6, null },
                    { 7, null },
                    { 8, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spots_VehicleId",
                table: "Spots",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spots");

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "Id", "ArriveTime", "Brand", "Color", "Model", "RegNr", "VehicleType", "Wheels" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ferrari", "Red", "F40", "abc123", 0, 4L },
                    { 2, new DateTime(2024, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Toyota", "Yellow", "Supra", "def456", 0, 4L }
                });
        }
    }
}
