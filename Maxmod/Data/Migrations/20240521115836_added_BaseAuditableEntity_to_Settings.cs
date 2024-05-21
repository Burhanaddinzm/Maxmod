using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maxmod.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_BaseAuditableEntity_to_Settings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Settings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Settings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Settings");
        }
    }
}
