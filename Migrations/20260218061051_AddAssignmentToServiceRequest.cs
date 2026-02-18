using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedEmployeeId",
                table: "ServiceRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_AssignedEmployeeId",
                table: "ServiceRequests",
                column: "AssignedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Employees_AssignedEmployeeId",
                table: "ServiceRequests",
                column: "AssignedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Employees_AssignedEmployeeId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_AssignedEmployeeId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "AssignedEmployeeId",
                table: "ServiceRequests");
        }
    }
}
