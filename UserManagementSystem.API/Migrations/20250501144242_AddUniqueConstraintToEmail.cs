using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "Ali123!!!", "+905012345678" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "Ayse123!!!", "+905112345678" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "aliveli", "05333333333" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Password", "Phone" },
                values: new object[] { "123456", "0533333334" });
        }
    }
}
