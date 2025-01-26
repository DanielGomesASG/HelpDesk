using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_TicketMessages_AddColumn_Origin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Origin",
                table: "TicketMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "TicketMessages");
        }
    }
}
