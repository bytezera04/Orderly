using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orderly.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sales",
                table: "ProductTbl",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sales",
                table: "ProductTbl");
        }
    }
}
