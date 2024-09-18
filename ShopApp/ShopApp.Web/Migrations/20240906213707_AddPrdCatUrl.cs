using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPrdCatUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Categories");
        }
    }
}
