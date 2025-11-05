using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class StaticSeedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Xvx6qlrZBLDld8UWBMF72up5Cf4Y7a1/au3Oo6nt7dzQWuQoGGHpq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$NT4SrHDw6QXy3/7kgVyumupgbY21LY6UuoEnD360ytPKHxl9DX8yu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "Company@123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$rkp0yOhD2vyfrfys7ny1e.Yd30Sw3XnkO/djrSlWn0zNXTXblgMIG");
        }
    }
}
