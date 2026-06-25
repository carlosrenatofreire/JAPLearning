using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JAPLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLoginTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "E_Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginCount",
                table: "E_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "E_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "E_Users");

            migrationBuilder.DropColumn(
                name: "LoginCount",
                table: "E_Users");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "E_Users");
        }
    }
}
