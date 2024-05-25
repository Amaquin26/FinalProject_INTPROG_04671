using FashionWebsite.Models;
using FashionWebsite.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FashionWebsite.Repository
{
    public class HomeRepository(ApplicationDbContext context) : IHomeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<TopDesignViewModel>> GetTopDesigns()
        {
            var designs = await _context.Designs
                .Include(d => d.User)
                .Select(d => new TopDesignViewModel
                {
                    FashionistaName = $"{d.User.FirstName} {d.User.LastName}",
                    FasionistaId = d.UserId,
                    DesignId = d.Id,
                    DesignImage = d.ImagePath,
                    DesignName = d.DesignName,
                    UpVotes = d.UpVotes 
                })
                .OrderByDescending(d => d.UpVotes)
                .Take(5)
                .ToListAsync();

            return designs;
        }
    }   
}
