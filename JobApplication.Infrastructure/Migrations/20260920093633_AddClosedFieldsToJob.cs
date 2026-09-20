using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClosedFieldsToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClosedBy",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                table: "Jobs");
        }
    }
}
