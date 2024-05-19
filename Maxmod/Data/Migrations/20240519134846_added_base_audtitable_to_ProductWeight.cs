using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maxmod.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_base_audtitable_to_ProductWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductWeights",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductWeights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "ProductWeights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProductWeights",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ProductWeights",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductWeights");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductWeights");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "ProductWeights");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProductWeights");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProductWeights");
        }
    }
}
