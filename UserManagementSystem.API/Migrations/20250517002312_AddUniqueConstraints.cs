using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "IX_User_Email_CaseSensitive");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "Description",
                value: "System administrator role. Has all permissions.");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "Description",
                value: "Regular user role");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName_CaseSensitive",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_UserName_CaseSensitive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email_CaseSensitive",
                table: "Users",
                newName: "IX_Users_Email");
        }
    }
}
