using Microsoft.EntityFrameworkCore.Migrations;

namespace Taledynamic.Core.Migrations
{
    public partial class AddIsTelegramField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTelegramTable",
                table: "Tables",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTelegramTable",
                table: "Tables");
        }
    }
}
