using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class secondxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$kslrUbHq1XDXwwtcaQp1fO8pQKwn5rQqwleUeSfJ5dPtXwoRENLf2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$NT4SrHDw6QXy3/7kgVyumupgbY21LY6UuoEnD360ytPKHxl9DX8yu");
        }
    }
}
