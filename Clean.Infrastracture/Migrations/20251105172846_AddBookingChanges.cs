using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clean.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Company",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
