using Microsoft.EntityFrameworkCore;
using Places.Models;

namespace Places.DAL
{
    public class PlacesDbContext : DbContext
    {
        public DbSet<Place> Places { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Media> MediaFiles { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=places.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

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
                .WithMany(u => u.Answers) // Виправлено 'Answes' на 'Answers'
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Media>()
                .HasOne(m => m.Place)
                .WithMany(p => p.MediaFiles)
                .HasForeignKey(m => m.PlaceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Media>()
                .HasOne(m => m.User)
                .WithMany(u => u.MediaFiles)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}