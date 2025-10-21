using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MovieAppPortfolio.DataServiceLayer
{
    public class MyDbContext:DbContext
    {
        public DbSet<TitleBasic> Title_Basics { get; set; }
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
        }

    }
}
