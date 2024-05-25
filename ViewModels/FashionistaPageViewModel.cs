namespace FashionWebsite.ViewModels
{
    public class FashionistaPageViewModel
    {
        public string FashionistaId { get; set; }
        public string FashionistaName { get; set; }
        public string FashionistaContact { get; set; }
        public int TotalUpvotes { get; set; }
        public int TotalDesigns { get; set; }
        public List<DesignViewModels> DesignViews { get; set; }
    }
}
