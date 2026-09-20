using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruiterIdToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecruiterId",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Jobs");
        }
    }
}
