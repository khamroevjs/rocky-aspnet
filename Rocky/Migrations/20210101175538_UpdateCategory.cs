using Microsoft.EntityFrameworkCore.Migrations;

namespace Rocky.Migrations
{
    public partial class UpdateCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayModel",
                table: "Category",
                newName: "DisplayOrder");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Category",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "Category",
                newName: "DisplayModel");
        }
    }
}
