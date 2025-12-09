using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Furm_VlazDut",
                table: "Variants",
                newName: "Data_VlazDut");

            migrationBuilder.RenameColumn(
                name: "Furm_TrebZnTeor",
                table: "Variants",
                newName: "Data_TrebZnTeor");

            migrationBuilder.RenameColumn(
                name: "Furm_TPerepad",
                table: "Variants",
                newName: "Data_TPerepad");

            migrationBuilder.RenameColumn(
                name: "Furm_TDut",
                table: "Variants",
                newName: "Data_TDut");

            migrationBuilder.RenameColumn(
                name: "Furm_SodKislorod",
                table: "Variants",
                newName: "Data_SodKislorod");

            migrationBuilder.RenameColumn(
                name: "Furm_RashVodiNaF",
                table: "Variants",
                newName: "Data_RashVodiNaF");

            migrationBuilder.RenameColumn(
                name: "Furm_RashGazNaF",
                table: "Variants",
                newName: "Data_RashGazNaF");

            migrationBuilder.RenameColumn(
                name: "Furm_RashDut",
                table: "Variants",
                newName: "Data_RashDut");

            migrationBuilder.RenameColumn(
                name: "Furm_NRabFurm",
                table: "Variants",
                newName: "Data_NRabFurm");

            migrationBuilder.RenameColumn(
                name: "Furm_KoefSzhatOchag",
                table: "Variants",
                newName: "Data_KoefSzhatOchag");

            migrationBuilder.RenameColumn(
                name: "Furm_HeightFurm",
                table: "Variants",
                newName: "Data_HeightFurm");

            migrationBuilder.RenameColumn(
                name: "Furm_GornDiam",
                table: "Variants",
                newName: "Data_GornDiam");

            migrationBuilder.RenameColumn(
                name: "Furm_FurmPodachaDutya",
                table: "Variants",
                newName: "Data_FurmPodachaDutya");

            migrationBuilder.RenameColumn(
                name: "Furm_DiamFurm",
                table: "Variants",
                newName: "Data_DiamFurm");

            migrationBuilder.RenameColumn(
                name: "Furm_DavlDut",
                table: "Variants",
                newName: "Data_DavlDut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_VlazDut",
                table: "Variants",
                newName: "Furm_VlazDut");

            migrationBuilder.RenameColumn(
                name: "Data_TrebZnTeor",
                table: "Variants",
                newName: "Furm_TrebZnTeor");

            migrationBuilder.RenameColumn(
                name: "Data_TPerepad",
                table: "Variants",
                newName: "Furm_TPerepad");

            migrationBuilder.RenameColumn(
                name: "Data_TDut",
                table: "Variants",
                newName: "Furm_TDut");

            migrationBuilder.RenameColumn(
                name: "Data_SodKislorod",
                table: "Variants",
                newName: "Furm_SodKislorod");

            migrationBuilder.RenameColumn(
                name: "Data_RashVodiNaF",
                table: "Variants",
                newName: "Furm_RashVodiNaF");

            migrationBuilder.RenameColumn(
                name: "Data_RashGazNaF",
                table: "Variants",
                newName: "Furm_RashGazNaF");

            migrationBuilder.RenameColumn(
                name: "Data_RashDut",
                table: "Variants",
                newName: "Furm_RashDut");

            migrationBuilder.RenameColumn(
                name: "Data_NRabFurm",
                table: "Variants",
                newName: "Furm_NRabFurm");

            migrationBuilder.RenameColumn(
                name: "Data_KoefSzhatOchag",
                table: "Variants",
                newName: "Furm_KoefSzhatOchag");

            migrationBuilder.RenameColumn(
                name: "Data_HeightFurm",
                table: "Variants",
                newName: "Furm_HeightFurm");

            migrationBuilder.RenameColumn(
                name: "Data_GornDiam",
                table: "Variants",
                newName: "Furm_GornDiam");

            migrationBuilder.RenameColumn(
                name: "Data_FurmPodachaDutya",
                table: "Variants",
                newName: "Furm_FurmPodachaDutya");

            migrationBuilder.RenameColumn(
                name: "Data_DiamFurm",
                table: "Variants",
                newName: "Furm_DiamFurm");

            migrationBuilder.RenameColumn(
                name: "Data_DavlDut",
                table: "Variants",
                newName: "Furm_DavlDut");
        }
    }
}
