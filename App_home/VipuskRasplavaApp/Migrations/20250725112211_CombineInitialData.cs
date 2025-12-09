using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class CombineInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Furm_GornDiam",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 10.0);

            migrationBuilder.AddColumn<double>(
                name: "Furm_HeightFurm",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 350.0);

            migrationBuilder.AddColumn<double>(
                name: "Furm_KoefSzhatOchag",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.75);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furm_GornDiam",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_HeightFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_KoefSzhatOchag",
                table: "Variants");
        }
    }
}
