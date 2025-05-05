using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class removeUnnecesaryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Courses_CourseId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Courses_CourseId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_CourseId",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CourseId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Articles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_CourseId",
                table: "Media",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CourseId",
                table: "Articles",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Courses_CourseId",
                table: "Articles",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Courses_CourseId",
                table: "Media",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
