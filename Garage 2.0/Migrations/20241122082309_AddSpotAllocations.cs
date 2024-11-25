using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotAllocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotVehicle");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Spots");

            migrationBuilder.CreateTable(
                name: "SpotAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    Fraction = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpotAllocations_Spots_SpotId",
                        column: x => x.SpotId,
                        principalTable: "Spots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpotAllocations_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpotAllocations_SpotId",
                table: "SpotAllocations",
                column: "SpotId");

            migrationBuilder.CreateIndex(
                name: "IX_SpotAllocations_VehicleId",
                table: "SpotAllocations",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotAllocations");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Spots",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpotVehicle",
                columns: table => new
                {
                    SpotsId = table.Column<int>(type: "int", nullable: false),
                    VehiclesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotVehicle", x => new { x.SpotsId, x.VehiclesId });
                    table.ForeignKey(
                        name: "FK_SpotVehicle_Spots_SpotsId",
                        column: x => x.SpotsId,
                        principalTable: "Spots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpotVehicle_Vehicle_VehiclesId",
                        column: x => x.VehiclesId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 1,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 2,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 3,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 4,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 5,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 6,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 7,
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Spots",
                keyColumn: "Id",
                keyValue: 8,
                column: "VehicleId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_SpotVehicle_VehiclesId",
                table: "SpotVehicle",
                column: "VehiclesId");
        }
    }
}
