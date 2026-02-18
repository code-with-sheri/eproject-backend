using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject_backend.Migrations
{
    /// <inheritdoc />
    public partial class mymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VacancyId",
                table: "Applications",
                newName: "vacancyId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Applications",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Applications",
                newName: "email");

            migrationBuilder.AddColumn<string>(
                name: "CvPath",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvPath",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "vacancyId",
                table: "Applications",
                newName: "VacancyId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Applications",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Applications",
                newName: "Email");
        }
    }
}
