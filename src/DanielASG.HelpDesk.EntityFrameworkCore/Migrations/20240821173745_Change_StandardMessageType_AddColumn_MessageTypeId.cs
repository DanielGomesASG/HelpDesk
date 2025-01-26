using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanielASG.HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class Change_StandardMessageType_AddColumn_MessageTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageTypeId",
                table: "StandardMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandardMessages_MessageTypeId",
                table: "StandardMessages",
                column: "MessageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StandardMessages_MessageTypes_MessageTypeId",
                table: "StandardMessages",
                column: "MessageTypeId",
                principalTable: "MessageTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StandardMessages_MessageTypes_MessageTypeId",
                table: "StandardMessages");

            migrationBuilder.DropIndex(
                name: "IX_StandardMessages_MessageTypeId",
                table: "StandardMessages");

            migrationBuilder.DropColumn(
                name: "MessageTypeId",
                table: "StandardMessages");
        }
    }
}
