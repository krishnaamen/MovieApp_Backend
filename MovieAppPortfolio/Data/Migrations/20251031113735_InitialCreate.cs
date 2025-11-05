using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieAppPortfolio.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                schema: "movie_app",
                table: "app_user");

            migrationBuilder.CreateTable(
                name: "name_rating",
                schema: "movie_app",
                columns: table => new
                {
                    nconst = table.Column<string>(type: "text", nullable: false),
                    weighted_rating = table.Column<decimal>(type: "numeric", nullable: true),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NameBasicnconst = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_name_rating", x => x.nconst);
                    table.ForeignKey(
                        name: "FK_name_rating_name_basics_NameBasicnconst",
                        column: x => x.NameBasicnconst,
                        principalSchema: "movie_app",
                        principalTable: "name_basics",
                        principalColumn: "nconst");
                });

            migrationBuilder.CreateIndex(
                name: "IX_name_rating_NameBasicnconst",
                schema: "movie_app",
                table: "name_rating",
                column: "NameBasicnconst");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "name_rating",
                schema: "movie_app");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                schema: "movie_app",
                table: "app_user",
                type: "text",
                nullable: true);
        }
    }
}
