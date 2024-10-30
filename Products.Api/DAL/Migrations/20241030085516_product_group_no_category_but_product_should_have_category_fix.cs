using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class product_group_no_category_but_product_should_have_category_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_Categories_CategoryName",
                table: "ProductGroups");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_CategoryName",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "ProductGroups");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryName",
                table: "Products",
                column: "CategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryName",
                table: "Products",
                column: "CategoryName",
                principalTable: "Categories",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryName",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "ProductGroups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_CategoryName",
                table: "ProductGroups",
                column: "CategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_Categories_CategoryName",
                table: "ProductGroups",
                column: "CategoryName",
                principalTable: "Categories",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
