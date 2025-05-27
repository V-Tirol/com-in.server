using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class addAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "publishedById",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publishedById",
                table: "Announcements");
        }
    }
}
