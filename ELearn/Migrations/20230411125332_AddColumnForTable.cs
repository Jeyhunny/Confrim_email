using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.Migrations
{
    public partial class AddColumnForTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Owners");
        }
    }
}
