using Microsoft.EntityFrameworkCore;
using Places.Models;

namespace Places.DAL.Repositories
{
    public class PlacesDbContext : DbContext
    {
        public PlacesDbContext(DbContextOptions<PlacesDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=D:\\Rider_project\\Places\\Places.WebAPI\\places.db")
                    .UseLazyLoadingProxies();
            }*/
        }
        
        public DbSet<Place> Places { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Place)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlaceId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Place)
                .WithMany(p => p.Questions)
                .HasForeignKey(q => q.PlaceId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Media>()
                .HasOne(m => m.Place)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(m => m.PlaceId);

            modelBuilder.Entity<Media>()
                .HasOne(m => m.User)
                .WithMany(u => u.MediaFiles)
                .HasForeignKey(m => m.UserId);
        }
    }
}
