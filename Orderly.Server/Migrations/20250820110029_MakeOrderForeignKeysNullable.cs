using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orderly.Server.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrderForeignKeysNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTbl_ProductTbl_ProductId",
                table: "OrderTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTbl_UserTbl_CustomerId",
                table: "OrderTbl");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTbl_ProductTbl_ProductId",
                table: "OrderTbl",
                column: "ProductId",
                principalTable: "ProductTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTbl_UserTbl_CustomerId",
                table: "OrderTbl",
                column: "CustomerId",
                principalTable: "UserTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTbl_ProductTbl_ProductId",
                table: "OrderTbl");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTbl_UserTbl_CustomerId",
                table: "OrderTbl");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTbl_ProductTbl_ProductId",
                table: "OrderTbl",
                column: "ProductId",
                principalTable: "ProductTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTbl_UserTbl_CustomerId",
                table: "OrderTbl",
                column: "CustomerId",
                principalTable: "UserTbl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
