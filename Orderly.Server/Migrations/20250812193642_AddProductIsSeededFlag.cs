using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orderly.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIsSeededFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeeded",
                table: "ProductTbl",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeeded",
                table: "ProductTbl");
        }
    }
}
