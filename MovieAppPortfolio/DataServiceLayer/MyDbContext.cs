using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer.Data;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class MyDbContext : DbContext
    {
        // Constructor for dependency injection
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //Optional: parameterless constructor for OnConfiguring fallback
        public MyDbContext() { }

        public DbSet<TitleBasic> Title_Basics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //  Load .env file and read DB_CONNECTION
                Env.Load();
                var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string not found. Please check your .env file.");
                }

                Console.WriteLine($"✅ Loaded Connection String: {connectionString}");
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TitleBasic
            modelBuilder.Entity<TitleBasic>(entity =>
            {
                entity.ToTable("title_basics", "movie_app");
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

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("app_user", "movie_app");
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedOnAdd();

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

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
