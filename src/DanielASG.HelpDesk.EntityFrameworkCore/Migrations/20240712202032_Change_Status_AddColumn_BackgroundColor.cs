using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_Status_AddColumn_BackgroundColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Statuses");
        }
    }
}
