using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoUrlToTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "P_Teachers",
                type: "varchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "P_Teachers");
        }
    }
}
