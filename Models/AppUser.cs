using Microsoft.AspNetCore.Identity;

namespace FashionWebsite.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Design> Designs { get; set; } = new List<Design>();
        public ICollection<UpVotes> UpVotes { get; set; } = new List<UpVotes>();
    }
}
