using FashionWebsite.ViewModels;

namespace FashionWebsite.Repository
{
    public interface IHomeRepository
    {
        Task<List<TopDesignViewModel>> GetTopDesigns();
    }
}
