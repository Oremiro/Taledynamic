using Microsoft.EntityFrameworkCore.Migrations;

namespace Taledynamic.Core.Migrations
{
    public partial class TgId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TgUsername",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TgUsername",
                table: "Users");
        }
    }
}
