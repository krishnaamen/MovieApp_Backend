using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class UserTokenAndFrameworkUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                schema: "movie_app",
                table: "app_user",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                schema: "movie_app",
                table: "app_user");
        }
    }
}
