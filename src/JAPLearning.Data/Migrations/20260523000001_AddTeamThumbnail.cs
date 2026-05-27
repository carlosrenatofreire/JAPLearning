using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "P_Teams",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "P_Teams");
        }
    }
}
