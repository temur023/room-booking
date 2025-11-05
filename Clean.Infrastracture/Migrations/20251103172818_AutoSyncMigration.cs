using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AutoSyncMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "UserName" },
                values: new object[] { "Super Admin", "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Name", "PasswordHash" },
                values: new object[] { 1, "SmartMeeting HQ", "$2a$11$Xvx6qlrZBLDld8UWBMF72up5Cf4Y7a1/au3Oo6nt7dzQWuQoGGHpq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "UserName" },
                values: new object[] { "System Administrator", "admin" });
        }
    }
}
