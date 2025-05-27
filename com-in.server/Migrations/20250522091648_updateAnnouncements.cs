using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class updateAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_announcementCategories",
                table: "announcementCategories");

            migrationBuilder.RenameTable(
                name: "announcementCategories",
                newName: "AnnouncementCategories");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                table: "AnnouncementCategories",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnouncementCategories",
                table: "AnnouncementCategories",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isPublished = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnouncementCategories",
                table: "AnnouncementCategories");

            migrationBuilder.RenameTable(
                name: "AnnouncementCategories",
                newName: "announcementCategories");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                table: "announcementCategories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_announcementCategories",
                table: "announcementCategories",
                column: "id");
        }
    }
}
