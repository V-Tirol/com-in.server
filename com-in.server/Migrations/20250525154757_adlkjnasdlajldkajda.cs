using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com_in.server.Migrations
{
    /// <inheritdoc />
    public partial class adlkjnasdlajldkajda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_courseId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "courseId",
                table: "Students",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_courseId",
                table: "Students",
                newName: "IX_Students_CourseId");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "course",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "course",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Students",
                newName: "courseId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                newName: "IX_Students_courseId");

            migrationBuilder.AlterColumn<int>(
                name: "courseId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Faculties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_courseId",
                table: "Students",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
