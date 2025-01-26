using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_Ticket_AddColumn_StaffUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StaffUserId",
                table: "Tickets",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StaffUserId",
                table: "Tickets",
                column: "StaffUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AbpUsers_StaffUserId",
                table: "Tickets",
                column: "StaffUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AbpUsers_StaffUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_StaffUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "StaffUserId",
                table: "Tickets");
        }
    }
}
