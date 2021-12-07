using Microsoft.EntityFrameworkCore.Migrations;

namespace Taledynamic.Core.Migrations
{
    public partial class AddMongoDbReferenceToTableTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MongoDbUId",
                table: "Tables",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MongoDbUId",
                table: "Tables");
        }
    }
}
