using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class aishdalkdjaldada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageURL",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageURL",
                table: "Announcements");
        }
    }
}
