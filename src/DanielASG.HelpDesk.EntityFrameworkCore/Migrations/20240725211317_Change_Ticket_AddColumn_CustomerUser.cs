using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_Ticket_AddColumn_CustomerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerUserId",
                table: "Tickets",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerUserId",
                table: "Tickets",
                column: "CustomerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AbpUsers_CustomerUserId",
                table: "Tickets",
                column: "CustomerUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AbpUsers_CustomerUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CustomerUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CustomerUserId",
                table: "Tickets");
        }
    }
}
