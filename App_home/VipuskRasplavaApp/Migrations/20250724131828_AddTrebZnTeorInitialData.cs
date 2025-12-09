using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTrebZnTeorInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Furm_TrebZnTeor",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 2150.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furm_TrebZnTeor",
                table: "Variants");
        }
    }
}
