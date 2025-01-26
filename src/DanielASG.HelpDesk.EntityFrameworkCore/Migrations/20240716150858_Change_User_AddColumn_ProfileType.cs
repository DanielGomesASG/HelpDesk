using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_User_AddColumn_ProfileType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileType",
                table: "AbpUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileType",
                table: "AbpUsers");
        }
    }
}
