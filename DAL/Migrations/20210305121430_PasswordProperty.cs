using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PasswordProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListedPlaneModel");

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "User",
                type: "binary(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.CreateTable(
                name: "ListedPlaneModel",
                columns: table => new
                {
                    PlaneModelId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ListedPlane_PlaneModel",
                        column: x => x.PlaneModelId,
                        principalTable: "PlaneModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListedPlane_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListedPlaneModel_PlaneModelId",
                table: "ListedPlaneModel",
                column: "PlaneModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ListedPlaneModel_UserId",
                table: "ListedPlaneModel",
                column: "UserId");
        }
    }
}
