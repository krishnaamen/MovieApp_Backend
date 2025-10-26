using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer.Data;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class MyDbContext : DbContext
    {
        // Add this constructor for dependency injection
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        // Keep the parameterless constructor for your current configuration
        public MyDbContext()
        {
        }

        public DbSet<TitleBasic> Title_Basics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                Env.Load();
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                Console.WriteLine($"Connection String: {connectionString}");
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // Configure User entity
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
                
                // unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}