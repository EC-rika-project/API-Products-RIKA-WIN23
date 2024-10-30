using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_ingress_and_coverimage_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ingress",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Ingress",
                table: "Products");
        }
    }
}
