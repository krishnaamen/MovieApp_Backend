using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieAppPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class InitialFrameworkModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "movie_app");

            migrationBuilder.CreateTable(
                name: "app_user",
                schema: "movie_app",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                schema: "movie_app",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    genre_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "genre_title",
                schema: "movie_app",
                columns: table => new
                {
                    genre_title_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tconst = table.Column<string>(type: "text", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre_title", x => x.genre_title_id);
                });

            migrationBuilder.CreateTable(
                name: "name_basics",
                schema: "movie_app",
                columns: table => new
                {
                    nconst = table.Column<string>(type: "text", nullable: false),
                    primary_name = table.Column<string>(type: "text", nullable: true),
                    birth_year = table.Column<int>(type: "integer", nullable: true),
                    death_year = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_name_basics", x => x.nconst);
                });

            migrationBuilder.CreateTable(
                name: "omdb_data",
                schema: "movie_app",
                columns: table => new
                {
                    tconst = table.Column<string>(type: "text", nullable: false),
                    poster = table.Column<string>(type: "text", nullable: true),
                    plot = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_omdb_data", x => x.tconst);
                });

            migrationBuilder.CreateTable(
                name: "title_basics",
                schema: "movie_app",
                columns: table => new
                {
                    tconst = table.Column<string>(type: "text", nullable: false),
                    title_type = table.Column<string>(type: "text", nullable: true),
                    primary_title = table.Column<string>(type: "text", nullable: true),
                    original_title = table.Column<string>(type: "text", nullable: true),
                    is_adult = table.Column<bool>(type: "boolean", nullable: true),
                    start_year = table.Column<int>(type: "integer", nullable: true),
                    end_year = table.Column<int>(type: "integer", nullable: true),
                    runtime_minutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_title_basics", x => x.tconst);
                });

            migrationBuilder.CreateTable(
                name: "title_principals",
                schema: "movie_app",
                columns: table => new
                {
                    tconst = table.Column<string>(type: "text", nullable: false),
                    nconst = table.Column<string>(type: "text", nullable: false),
                    ordering = table.Column<int>(type: "integer", nullable: false),
                    category = table.Column<string>(type: "text", nullable: true),
                    job = table.Column<string>(type: "text", nullable: true),
                    characters = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_title_principals", x => new { x.tconst, x.nconst, x.ordering });
                });

            migrationBuilder.CreateTable(
                name: "title_ratings",
                schema: "movie_app",
                columns: table => new
                {
                    tconst = table.Column<string>(type: "text", nullable: false),
                    average_rating = table.Column<decimal>(type: "numeric", nullable: true),
                    num_votes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_title_ratings", x => x.tconst);
                });

            migrationBuilder.CreateTable(
                name: "search_history",
                schema: "movie_app",
                columns: table => new
                {
                    search_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    search_query = table.Column<string>(type: "text", nullable: false),
                    search_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_search_history", x => x.search_id);
                    table.ForeignKey(
                        name: "FK_search_history_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "movie_app",
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookmark",
                schema: "movie_app",
                columns: table => new
                {
                    bookmark_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    tconst = table.Column<string>(type: "text", nullable: true),
                    nconst = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookmark", x => x.bookmark_id);
                    table.ForeignKey(
                        name: "FK_bookmark_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "movie_app",
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookmark_name_basics_nconst",
                        column: x => x.nconst,
                        principalSchema: "movie_app",
                        principalTable: "name_basics",
                        principalColumn: "nconst");
                    table.ForeignKey(
                        name: "FK_bookmark_title_basics_tconst",
                        column: x => x.tconst,
                        principalSchema: "movie_app",
                        principalTable: "title_basics",
                        principalColumn: "tconst");
                });

            migrationBuilder.CreateTable(
                name: "user_note",
                schema: "movie_app",
                columns: table => new
                {
                    note_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    tconst = table.Column<string>(type: "text", nullable: true),
                    nconst = table.Column<string>(type: "text", nullable: true),
                    note_text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_note", x => x.note_id);
                    table.ForeignKey(
                        name: "FK_user_note_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "movie_app",
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_note_name_basics_nconst",
                        column: x => x.nconst,
                        principalSchema: "movie_app",
                        principalTable: "name_basics",
                        principalColumn: "nconst");
                    table.ForeignKey(
                        name: "FK_user_note_title_basics_tconst",
                        column: x => x.tconst,
                        principalSchema: "movie_app",
                        principalTable: "title_basics",
                        principalColumn: "tconst");
                });

            migrationBuilder.CreateTable(
                name: "user_rating",
                schema: "movie_app",
                columns: table => new
                {
                    rating_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    tconst = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_rating", x => x.rating_id);
                    table.ForeignKey(
                        name: "FK_user_rating_app_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "movie_app",
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_rating_title_basics_tconst",
                        column: x => x.tconst,
                        principalSchema: "movie_app",
                        principalTable: "title_basics",
                        principalColumn: "tconst",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_user_email",
                schema: "movie_app",
                table: "app_user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_user_username",
                schema: "movie_app",
                table: "app_user",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_nconst",
                schema: "movie_app",
                table: "bookmark",
                column: "nconst");

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_tconst",
                schema: "movie_app",
                table: "bookmark",
                column: "tconst");

            migrationBuilder.CreateIndex(
                name: "IX_bookmark_user_id",
                schema: "movie_app",
                table: "bookmark",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_search_history_user_id",
                schema: "movie_app",
                table: "search_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_note_nconst",
                schema: "movie_app",
                table: "user_note",
                column: "nconst");

            migrationBuilder.CreateIndex(
                name: "IX_user_note_tconst",
                schema: "movie_app",
                table: "user_note",
                column: "tconst");

            migrationBuilder.CreateIndex(
                name: "IX_user_note_user_id",
                schema: "movie_app",
                table: "user_note",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_rating_tconst",
                schema: "movie_app",
                table: "user_rating",
                column: "tconst");

            migrationBuilder.CreateIndex(
                name: "IX_user_rating_user_id",
                schema: "movie_app",
                table: "user_rating",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmark",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "genre",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "genre_title",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "omdb_data",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "search_history",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "title_principals",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "title_ratings",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "user_note",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "user_rating",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "name_basics",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "app_user",
                schema: "movie_app");

            migrationBuilder.DropTable(
                name: "title_basics",
                schema: "movie_app");
        }
    }
}
