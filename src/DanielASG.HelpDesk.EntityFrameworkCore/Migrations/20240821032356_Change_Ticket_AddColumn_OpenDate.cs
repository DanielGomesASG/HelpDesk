using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_Ticket_AddColumn_OpenDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OpenDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenDate",
                table: "Tickets");
        }
    }
}
