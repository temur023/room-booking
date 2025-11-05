using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "FullName", "PasswordHash", "Role", "UserName" },
                values: new object[] { 1, 0, "Super Admin", "$2a$11$kslrUbHq1XDXwwtcaQp1fO8pQKwn5rQqwleUeSfJ5dPtXwoRENLf2", 2, "superadmin" });
        }
    }
}
