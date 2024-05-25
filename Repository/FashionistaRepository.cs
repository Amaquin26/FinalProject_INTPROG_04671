using FashionWebsite.Models;
using FashionWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FashionWebsite.Repository
{
    public class FashionistaRepository(ApplicationDbContext context, UserManager<AppUser> userManager) : IFashionistaRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<List<FashionistaViewModel>> GetAllFashionista()
        {

            var users = await _context.Users
                .Include(x => x.Designs)
                .Select(x => new FashionistaViewModel
                {
                    FashionistaId = x.Id,
                    FashionistaName = $"{x.FirstName} {x.LastName}",
                    FashionistaContact = x.PhoneNumber,
                    TotalUpvotes = x.Designs.Select(x => x.UpVotes).Sum(),
                    TotalDesigns = x.Designs.Count()
                })
                .OrderByDescending(x => x.TotalUpvotes)
                .ToListAsync();

            var fashionistas = new List<FashionistaViewModel>();

            foreach(var u in users)
            {
                var user = await _userManager.FindByIdAsync(u.FashionistaId);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Fashionista"))
                    fashionistas.Add(u);
            }

            return fashionistas;
        }

        public async Task<FashionistaPageViewModel> GetFashionistaPage(string userId)
        {
            var fashionistaPageModel = new FashionistaPageViewModel();

            var fashionista = await _context.Users.Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (fashionista == null)
            {
                return null;
            }

            fashionistaPageModel.FashionistaId = fashionista.Id;
            fashionistaPageModel.FashionistaName = $"{fashionista.FirstName} {fashionista.LastName}";
            fashionistaPageModel.FashionistaContact = fashionista.PhoneNumber;
        
            var designs = await _context.Designs
                .Where(x => x.UserId == fashionista.Id)
                .Select(d => new DesignViewModels
                {
                    Id = d.Id,
                    Name = d.DesignName,
                    Image = d.ImagePath,
                    UpVotes = d.UpVotes,
                    FashionistaId = d.UserId,
                    FashionistaName = $"{d.User.FirstName} {d.User.LastName}"
                })
                .OrderByDescending(x => x.UpVotes)
                .ToListAsync();

            fashionistaPageModel.DesignViews = designs;
            fashionistaPageModel.TotalUpvotes = designs.Select(x => x.UpVotes).Sum();
            fashionistaPageModel.TotalDesigns = designs.Count();

            return fashionistaPageModel;
        }
    }
}
