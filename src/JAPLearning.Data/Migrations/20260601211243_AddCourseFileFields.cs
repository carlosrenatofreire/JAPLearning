using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseFileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfFileUrl",
                table: "E_Courses",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SnapshotUrl",
                table: "E_Courses",
                type: "varchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfFileUrl",
                table: "E_Courses");

            migrationBuilder.DropColumn(
                name: "SnapshotUrl",
                table: "E_Courses");
        }
    }
}
