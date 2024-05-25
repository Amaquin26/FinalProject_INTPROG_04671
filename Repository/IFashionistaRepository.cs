using FashionWebsite.ViewModels;

namespace FashionWebsite.Repository
{
    public interface IFashionistaRepository
    {
        Task<List<FashionistaViewModel>> GetAllFashionista();
        Task<FashionistaPageViewModel> GetFashionistaPage(string userId);
    }
}
