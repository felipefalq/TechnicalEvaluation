using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data.Map;
using technicalevaluation.Models;

namespace technicalevaluation.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        public DbSet<CollaboratorInfo> Collaborators { get; set; }
        public DbSet<UnitInfo> Unit { get; set; }
        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaboratorInfo>()
                .HasOne(c => c.Unit)
                .WithMany()
                .HasForeignKey(c => c.UnitId);

            modelBuilder.Entity<CollaboratorInfo>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    //.UseLazyLoadingProxies() // 
                    .UseNpgsql("Database");
            }
        }
    }
}
