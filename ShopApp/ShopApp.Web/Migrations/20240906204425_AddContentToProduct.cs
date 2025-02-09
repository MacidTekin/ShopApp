﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddContentToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Products");
        }
    }
}
