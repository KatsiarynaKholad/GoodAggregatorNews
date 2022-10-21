using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodAggregatorNews.Database.Migrations
{
    public partial class AddFieldFullnameOfComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fullname",
                table: "Comments");
        }
    }
}
