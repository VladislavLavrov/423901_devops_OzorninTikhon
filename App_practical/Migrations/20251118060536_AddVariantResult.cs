using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_practical.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Result",
                table: "Variants",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Variants");
        }
    }
}
