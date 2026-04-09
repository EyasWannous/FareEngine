using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FareEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FarePolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RatePerKm = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    FlatAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ZoneNumber = table.Column<int>(type: "int", nullable: true),
                    ZonePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarePolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Surcharge = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DistanceInKm = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    ZoneNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldProductFarePolicy",
                columns: table => new
                {
                    SoldProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarePolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProductFarePolicy", x => new { x.SoldProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_SoldProductFarePolicy_SoldProducts_SoldProductId",
                        column: x => x.SoldProductId,
                        principalTable: "SoldProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldProductModification",
                columns: table => new
                {
                    SoldProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProductModification", x => new { x.SoldProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_SoldProductModification_SoldProducts_SoldProductId",
                        column: x => x.SoldProductId,
                        principalTable: "SoldProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarePolicies");

            migrationBuilder.DropTable(
                name: "Modifications");

            migrationBuilder.DropTable(
                name: "SoldProductFarePolicy");

            migrationBuilder.DropTable(
                name: "SoldProductModification");

            migrationBuilder.DropTable(
                name: "SoldProducts");
        }
    }
}
