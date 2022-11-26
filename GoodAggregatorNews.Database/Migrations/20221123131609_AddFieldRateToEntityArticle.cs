using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodAggregatorNews.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldRateToEntityArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Articles",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Articles");
        }
    }
}
