using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYAcademey_Portfollo.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descriptipn",
                table: "HeroSections",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Tittle",
                table: "Experiences",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "HeroSections",
                newName: "Descriptipn");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Experiences",
                newName: "Tittle");
        }
    }
}
