using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspredeleniyeDutyaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFurmClosing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<bool>>(
                name: "Furm_FurmPodachaDutya",
                table: "Variants",
                type: "boolean[]",
                nullable: false,
                defaultValue: new List<bool> {
                    true, true, true, true, true,
                    false, true, true, true, true,
                    true, true, true, true, false,
                    true, true, true, true, true,
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Furm_FurmPodachaDutya",
                table: "Variants");
        }
    }
}
