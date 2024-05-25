using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FashionWebsite.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Design> Designs { get; set; }

        public DbSet<UpVotes> UpVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UpVotes>()
                    .HasKey(pc => new { pc.UserId, pc.DesignId });
            modelBuilder.Entity<UpVotes>()
                    .HasOne(p => p.AppUser)
                    .WithMany(pc => pc.UpVotes)
                    .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<UpVotes>()
                    .HasOne(p => p.Design)
                    .WithMany(pc => pc.UserUpVotes)
                    .HasForeignKey(c => c.DesignId);
        }
    }
}
