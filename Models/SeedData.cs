using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FashionWebsite.Models
{
    public class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            string fasionNistaId = "";

            ApplicationDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.UserRoles.Any())
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                    var contextService = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    contextService.Database.EnsureCreated();

                    if (!await roleManager.RoleExistsAsync(AppRole.Fashionista))
                        await roleManager.CreateAsync(new AppRole { Name = AppRole.Fashionista });
                    if (!await roleManager.RoleExistsAsync(AppRole.Customer))
                        await roleManager.CreateAsync(new AppRole { Name = AppRole.Customer });

                    //Users
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    string fashionistaEmail = "Fashionista@gmail.com";

                    var fashionistaUser = await userManager.FindByEmailAsync(fashionistaEmail);
                    if (fashionistaUser == null)
                    {
                        var newFashionistaUser = new AppUser()
                        {
                            UserName = "FashionistaOrig",
                            Email = fashionistaEmail,
                            EmailConfirmed = true,
                            FirstName = "Fashion",
                            LastName = "Nista",
                            PhoneNumber = "+639564421867",
                        };
                        await userManager.CreateAsync(newFashionistaUser, "Fashionista123!");
                        await userManager.AddToRoleAsync(newFashionistaUser, UserRoles.Fashionista);
                        fasionNistaId = newFashionistaUser.Id;

                        contextService.Designs.AddRange(
                            new Design
                            {
                                DesignName = "Cherry Blossom Japanese Jacket",
                                Description = "Experience the elegance of Japanese culture with our Cherry Blossom Japanese Jacket. This exquisite piece features delicate cherry blossom embroidery, symbolizing beauty and renewal. Crafted from high-quality materials, it combines traditional artistry with modern comfort. Perfect for adding a touch of sophistication to any outfit, this jacket is a timeless addition to your wardrobe.",
                                ImagePath = "/designImages//japanese_jacket.jpg",
                                Price = 1250,
                                UserId = fasionNistaId,
                                UpVotes = 126,
                                DateAdded = DateTime.Now,                           
                            },
                            new Design
                            {
                                DesignName = "Fashion Hat Retro Melon Skin",
                                Description = "Embrace vintage charm with our Retro Melon Skin Fashion Hat. Featuring a distinctive melon-shaped design and crafted from high-quality materials, this stylish accessory offers a nod to classic elegance with a contemporary twist, perfect for any fashion-forward wardrobe.",
                                ImagePath = "/designImages//hat_retro.webp",
                                Price = 2500,
                                UserId = fasionNistaId,
                                UpVotes = 5,
                                DateAdded = DateTime.Now
                            }
                        );
                    }        

                    string customerUserEmail = "customer@gmail.com";

                    var appUser = await userManager.FindByEmailAsync(customerUserEmail);
                    if (appUser == null)
                    {
                        var newCustomerUser = new AppUser()
                        {
                            UserName = "Customer",
                            Email = customerUserEmail,
                            EmailConfirmed = true,
                            FirstName = "Customer",
                            LastName = "Customer",
                        };
                        await userManager.CreateAsync(newCustomerUser, "Customer123!");
                        await userManager.AddToRoleAsync(newCustomerUser, UserRoles.Customer);
                    }

                    contextService.SaveChanges();
                }             
                   
            }
        }
    }
}
