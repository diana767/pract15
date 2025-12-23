using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pract15.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands$",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands$", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories$",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories$", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags$",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags$", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products$",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<double>(type: "float", nullable: true),
                    stock = table.Column<double>(type: "float", nullable: true),
                    rating = table.Column<double>(type: "float", nullable: true),
                    created_at = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    brand_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products$", x => x.id);
                    table.ForeignKey(
                        name: "FK_products$_brands$",
                        column: x => x.brand_id,
                        principalTable: "brands$",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_products$_categories$",
                        column: x => x.category_id,
                        principalTable: "categories$",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_tags$",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    tag_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__product___3213E83F8C27F16F", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_tags$_products$",
                        column: x => x.product_id,
                        principalTable: "products$",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_tags$_tags$",
                        column: x => x.tag_id,
                        principalTable: "tags$",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_tags$_product_id",
                table: "product_tags$",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_tags$_tag_id",
                table: "product_tags$",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_products$_brand_id",
                table: "products$",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_products$_category_id",
                table: "products$",
                column: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_tags$");

            migrationBuilder.DropTable(
                name: "products$");

            migrationBuilder.DropTable(
                name: "tags$");

            migrationBuilder.DropTable(
                name: "brands$");

            migrationBuilder.DropTable(
                name: "categories$");
        }
    }
}
