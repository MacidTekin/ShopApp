using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndOrderItemUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Users_UserId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_UserId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_UserId",
                table: "OrderItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Users_UserId",
                table: "OrderItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
