using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class dasdjnaslkdjasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPublished",
                table: "Announcements",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Announcements",
                newName: "isPublished");
        }
    }
}
