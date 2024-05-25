using FashionWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FashionWebsite.Repository
{
    public interface IDesignRepository
    {
        Task<List<MyDesignViewModel>> GetMyDesigns();   
        Task<List<DesignViewModels>> GetAllDesigns();   
        Task<bool> AddDesign(AddDesignViewModel design);
        Task<DesignPageViewModel> GetDesignById(int id);
        Task<bool> EditDesign(EditDesignViewModel design);
        Task UpvoteDesign(int id, int flag, string userId);
        Task<List<MyDesignViewModel>> GetUpvotedDesigns();
    }
}
