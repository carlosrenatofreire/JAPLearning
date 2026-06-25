using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppVersions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "E_AppVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<string>(type: "varchar(20)", nullable: false),
                    Title = table.Column<string>(type: "varchar(150)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_AppVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "E_AppVersionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_E_AppVersionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_E_AppVersionItems_E_AppVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "E_AppVersions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_E_AppVersionItems_VersionId",
                table: "E_AppVersionItems",
                column: "VersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "E_AppVersionItems");

            migrationBuilder.DropTable(
                name: "E_AppVersions");
        }
    }
}
