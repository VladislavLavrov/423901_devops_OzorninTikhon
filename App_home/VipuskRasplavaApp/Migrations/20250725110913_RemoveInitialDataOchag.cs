using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInitialDataOchag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ochag_DavlDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_DiamVozdFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_GornDiam",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_HeightVozdFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_KoefSzhatOchag",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_NRabFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_PoteriRashDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_RashDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_RashPrirGaz",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_ReacSposKoks",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_SutProizvDomPech",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Ochag_TDut",
                table: "Variants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Ochag_DavlDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_DiamVozdFurm",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_GornDiam",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_HeightVozdFurm",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_KoefSzhatOchag",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_NRabFurm",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_PoteriRashDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_RashDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_RashPrirGaz",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_ReacSposKoks",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_SutProizvDomPech",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Ochag_TDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
