using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orderly.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessagePublicId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "ChatMessageTbl",
                type: "varchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "ChatMessageTbl");
        }
    }
}
