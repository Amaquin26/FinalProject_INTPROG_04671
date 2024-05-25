using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FashionWebsite.Models
{
    public class ApplicationDbContext :IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Design> Designs { get; set; }
    }
}
