using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieAppPortfolio.DataServiceLayer.entities;
using MovieAppPortfolio.DataServiceLayer.TitlePrincipal;
using MovieAppPortfolio.DataServiceLayer.user;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class MyDbContext:DbContext
    {
       
        public DbSet<TitleBasic> Title_Basics { get; set; }
        public DbSet<TitleRating> Title_Ratings { get; set; }
        public DbSet<OmdbData> Omdb_Data { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreTitle> Genre_Titles { get; set; }
        public DbSet<NameBasic> Name_Basics { get; set; }
        public DbSet<TitlePrincipals> Title_Principals { get; set; }

        public DbSet<User> Users { get; set; }


        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<UserRating> User_Ratings { get; set; }  // ADD THIS LINE
        public DbSet<SearchHistory> Search_History { get; set; }
        public DbSet<UserNote> User_Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            Console.WriteLine($"Connection String: {connectionString}");
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("movie_app");
            modelBuilder.Entity<TitleBasic>(entity =>
            {
                entity.ToTable("title_basics","movie_app");
                entity.HasKey(e => e.tconst);

                entity.Property(e => e.tconst).HasColumnName("tconst");
                entity.Property(e => e.titleType).HasColumnName("title_type");
                entity.Property(e => e.primaryTitle).HasColumnName("primary_title");
                entity.Property(e => e.originalTitle).HasColumnName("original_title");
                entity.Property(e => e.isAdult).HasColumnName("is_adult");
                entity.Property(e => e.startYear).HasColumnName("start_year");
                entity.Property(e => e.endYear).HasColumnName("end_year");
                entity.Property(e => e.runtimeMinutes).HasColumnName("runtime_minutes");

            });

            modelBuilder.Entity<TitleRating>(entity =>
            {
                entity.ToTable("title_ratings", "movie_app");
                entity.HasKey(e => e.tconst);

                entity.Property(e => e.tconst).HasColumnName("tconst");
                entity.Property(e => e.averageRating).HasColumnName("average_rating");
                entity.Property(e => e.numVotes).HasColumnName("num_votes");
            });


            modelBuilder.Entity<OmdbData>(entity =>
            {
                entity.ToTable("omdb_data", "movie_app");
                entity.HasKey(e => e.tconst);

                entity.Property(e => e.tconst).HasColumnName("tconst");
                entity.Property(e => e.poster).HasColumnName("poster");
                entity.Property(e => e.plot).HasColumnName("plot");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre", "movie_app");
                entity.HasKey(e => e.genreId);

                entity.Property(e => e.genreId).HasColumnName("genre_id");
                entity.Property(e => e.genreName).HasColumnName("genre_name");
            });


            modelBuilder.Entity<GenreTitle>(entity =>
            {
                entity.ToTable("genre_title", "movie_app");
                entity.HasKey(e => e.genreTitleId);

                entity.Property(e => e.genreTitleId).HasColumnName("genre_title_id");
                entity.Property(e => e.tconst).HasColumnName("tconst");
                entity.Property(e => e.genreId).HasColumnName("genre_id");
            });

            modelBuilder.Entity<NameBasic>(entity =>
            {
                entity.ToTable("name_basics", "movie_app");
                entity.HasKey(e => e.nconst);

                entity.Property(e => e.nconst).HasColumnName("nconst");
                entity.Property(e => e.primaryName).HasColumnName("primary_name");
                entity.Property(e => e.birthYear).HasColumnName("birth_year");
                entity.Property(e => e.deathYear).HasColumnName("death_year");
            });

            modelBuilder.Entity<TitlePrincipals>(entity =>
            {
                entity.ToTable("title_principals", "movie_app");
                entity.HasKey(e => new { e.tconst, e.nconst, e.ordering });

                entity.Property(e => e.tconst).HasColumnName("tconst");
                entity.Property(e => e.nconst).HasColumnName("nconst");
                entity.Property(e => e.ordering).HasColumnName("ordering");
                entity.Property(e => e.category).HasColumnName("category");
                entity.Property(e => e.job).HasColumnName("job");
                entity.Property(e => e.characters).HasColumnName("characters");
            });


            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.ToTable("bookmark", "movie_app");
                entity.HasKey(e => e.BookmarkId);

                // Column mappings only
                entity.Property(e => e.BookmarkId).HasColumnName("bookmark_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.TConst).HasColumnName("tconst");
                entity.Property(e => e.NConst).HasColumnName("nconst");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                // Relationships (navigation properties)
                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bookmarks)
                    .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.TitleBasic)
                    .WithMany()
                    .HasForeignKey(b => b.TConst);

                entity.HasOne(b => b.NameBasic)
                    .WithMany()
                    .HasForeignKey(b => b.NConst);
            });

            modelBuilder.Entity<UserRating>(entity =>
            {
                entity.ToTable("user_rating", "movie_app");
                entity.HasKey(e => e.RatingId);

                // Column mappings only
                entity.Property(e => e.RatingId).HasColumnName("rating_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.TConst).HasColumnName("tconst");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.RatedAt).HasColumnName("rated_at");

                // Relationships
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRatings)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.TitleBasic)
                    .WithMany()
                    .HasForeignKey(ur => ur.TConst);
            });

            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.ToTable("search_history", "movie_app");
                entity.HasKey(e => e.SearchId);

                // Column mappings only
                entity.Property(e => e.SearchId).HasColumnName("search_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.SearchQuery).HasColumnName("search_query");
                entity.Property(e => e.SearchTime).HasColumnName("search_time");

                // Relationships
                entity.HasOne(sh => sh.User)
                    .WithMany(u => u.SearchHistories)
                    .HasForeignKey(sh => sh.UserId);
            });


            modelBuilder.Entity<UserNote>(entity =>
            {
                entity.ToTable("user_note", "movie_app");
                entity.HasKey(e => e.NoteId);

                // Column mappings only
                entity.Property(e => e.NoteId).HasColumnName("note_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.TConst).HasColumnName("tconst");
                entity.Property(e => e.NConst).HasColumnName("nconst");
                entity.Property(e => e.NoteText).HasColumnName("note_text");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                // Relationships
                entity.HasOne(un => un.User)
                    .WithMany(u => u.UserNotes)
                    .HasForeignKey(un => un.UserId);

                entity.HasOne(un => un.TitleBasic)
                    .WithMany()
                    .HasForeignKey(un => un.TConst);

                entity.HasOne(un => un.NameBasic)
                    .WithMany()
                    .HasForeignKey(un => un.NConst);
            });











            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("app_user", "movie_app");

                // Correct way to configure primary key with column name
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedOnAdd(); // Add this if it's an auto-incrementing ID

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()")
                    .HasColumnName("created_at");

                entity.Ignore(e => e.Token);

                // unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });




        }

    }
}
