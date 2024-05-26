using FashionWebsite.Models;
using System.Xml.Linq;

namespace FashionWebsite.ViewModels
{
    public class DesignPageViewModel
    {
        public int DesignId { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public string DesignImage { get; set; }
        public string FashionistaName { get; set; }
        public string FasionistaId { get; set; }
        public int UpVotes { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsOwner { get; set; }
        public bool HasUpvoted { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
