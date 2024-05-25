using FashionWebsite.Models;
using FashionWebsite.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace FashionWebsite.Repository
{
    public class DesignRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment environment) :IDesignRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<List<MyDesignViewModel>> GetMyDesigns()
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }


            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;


            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var myDesigns = await _context.Designs
                .Where(d => d.UserId == userId).
                Select(d => new MyDesignViewModel
                {
                    Id = d.Id,
                    Name = d.DesignName,
                    Image = d.ImagePath,
                    UpVotes = d.UpVotes
                }).
                ToListAsync();

            return myDesigns;
        }

        public async Task<List<DesignViewModels>> GetAllDesigns()
        {
            var designs = await _context.Designs
                .Include(d => d.User)
                .Select(d => new DesignViewModels
                {
                    Id = d.Id,
                    Name = d.DesignName,
                    Image = d.ImagePath,
                    UpVotes = d.UpVotes,
                    FashionistaId = d.UserId,
                    FashionistaName = $"{d.User.FirstName} {d.User.LastName}"
                })
                .OrderByDescending(d => d.UpVotes)
                .ToListAsync();

            return designs;
        }

        public async Task<bool> AddDesign(AddDesignViewModel design)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }


            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;


            if (userClaim == null)
            {
                return false;
            }
            else
            {
                userId = userClaim.Value;
            }

            var newDesign = new Design
            {
                DesignName = design.Name,
                Description = design.Description,
                UserId = userId,
                Price = design.Price,
                DateAdded = DateTime.Now,
                UpVotes = 0
            };

            string imagePath = "";
            if (design.Image != null && design.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "designImages", userId);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Path.GetRandomFileName()}{Path.GetExtension(design.Image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await design.Image.CopyToAsync(fileStream);
                }

                imagePath = $"/designImages//{userId}//{uniqueFileName}";
            }
            else
            {
                return false;
            }

            newDesign.ImagePath = imagePath;
            _context.Designs.Add(newDesign);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditDesign(EditDesignViewModel designDetails)
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }


            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;


            if (userClaim == null)
            {
                return false;
            }
            else
            {
                userId = userClaim.Value;
            }

            var design = await _context.Designs.Where(d => d.Id == designDetails.Id && d.UserId == userId).FirstOrDefaultAsync();

            if (design == null)
                return false;


            design.DesignName = designDetails.Name;
            design.Description = designDetails.Description;
            design.Price = designDetails.Price;

            string imagePath = design.ImagePath;
            if (designDetails.Image != null && designDetails.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "designImages", userId);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Path.GetRandomFileName()}{Path.GetExtension(designDetails.Image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await designDetails.Image.CopyToAsync(fileStream);
                }

                imagePath = $"/designImages//{userId}//{uniqueFileName}";
            }

            design.ImagePath = imagePath;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DesignPageViewModel> GetDesignById(int id)
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;
            bool isOwner = false;
            bool hasUpvoted = false;


            var design = await _context.Designs
                .Where(d => d.Id == id)
                .Include(d => d.User)
                .Select(d => new DesignPageViewModel
                {
                    DesignId = d.Id,
                    DesignName = d.DesignName,
                    Description = d.Description,
                    DesignImage = d.ImagePath,
                    FashionistaName = $"{d.User.FirstName} {d.User.LastName}",
                    FasionistaId = d.UserId,
                    UpVotes = d.UpVotes,
                    Price = d.Price,
                    DateAdded = d.DateAdded,
                })
                .FirstOrDefaultAsync();

            if (userClaim != null)
            {
                userId = userClaim.Value;
                isOwner = await _context.Designs.Where(d => d.Id == id && d.UserId == userId).AnyAsync();
                hasUpvoted = await _context.UpVotes.Where(d => d.UserId == userId && d.DesignId == id).AnyAsync();
            }

            if(design != null) 
            { 
                design.IsOwner = isOwner;
                design.HasUpvoted = hasUpvoted;
            }

            return design;
        }

        public async Task UpvoteDesign(int id, int flag, string userId)
        {
            var upvote = await _context.UpVotes.Where(u => u.UserId == userId && u.DesignId == id).FirstOrDefaultAsync();
            var design = await _context.Designs.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (design == null) 
            {
                return;
            }

            if(flag == 1)
            {
                if (upvote == null)
                {
                    var newUpVote = new UpVotes
                    {
                        UserId = userId,
                        DesignId = id
                    };

                    _context.UpVotes.Add(newUpVote);
                    design.UpVotes += 1;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                if(upvote != null) 
                {
                    _context.UpVotes.Remove(upvote);
                    design.UpVotes -= 1;
                    await _context.SaveChangesAsync();
                }
            }

            return;
        }

        public async Task<List<MyDesignViewModel>> GetUpvotedDesigns()
        {
            if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }


            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userId = null;


            if (userClaim == null)
            {
                var returnUrl = _contextAccessor.HttpContext.Request.GetEncodedPathAndQuery();

                // Redirect to login, including the return URL for post-login redirection
                _contextAccessor.HttpContext.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(returnUrl));
            }
            else
            {
                userId = userClaim.Value;
            }

            var upvotedIds = await _context.Users
            .Where(d => d.Id == userId)
            .Include(d => d.UpVotes)
            .SelectMany(d => d.UpVotes.Select(v => v.DesignId))
            .ToListAsync();

            var myUpvotedDesings = await _context.Designs
            .Where(d => upvotedIds.Contains(d.Id))
            .Select(d => new MyDesignViewModel
            {
                Id = d.Id,
                Name = d.DesignName,
                Image = d.ImagePath,
                UpVotes = d.UpVotes
            })
            .ToListAsync();

            return myUpvotedDesings;
        }
    }
}
