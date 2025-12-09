using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialDataToVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Furm_DavlDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Furm_DiamFurm",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Furm_NRabFurm",
                table: "Variants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Furm_RashDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<List<double>>(
                name: "Furm_RashGazNaF",
                table: "Variants",
                type: "double precision[]",
                nullable: false);

            migrationBuilder.AddColumn<List<double>>(
                name: "Furm_RashVodiNaF",
                table: "Variants",
                type: "double precision[]",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "Furm_SodKislorod",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Furm_TDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<List<double>>(
                name: "Furm_TPerepad",
                table: "Variants",
                type: "double precision[]",
                nullable: false);

            migrationBuilder.AddColumn<double>(
                name: "Furm_VlazDut",
                table: "Variants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furm_DavlDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_DiamFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_NRabFurm",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_RashDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_RashGazNaF",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_RashVodiNaF",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_SodKislorod",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_TDut",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_TPerepad",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "Furm_VlazDut",
                table: "Variants");

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
    }
}
