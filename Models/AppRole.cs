using Microsoft.AspNetCore.Identity;

namespace FashionWebsite.Models
{
    public class AppRole : IdentityRole
    {
        public const string Fashionista = "Fashionista";
        public const string Customer = "Customer";
    }
}
