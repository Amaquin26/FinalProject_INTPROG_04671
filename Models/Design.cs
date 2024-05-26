using System.ComponentModel.DataAnnotations;

namespace FashionWebsite.Models
{
    public class Design
    {
        [Key]
        public int Id { get; set; }
        public string DesignName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public decimal Price { get; set; }
        public int UpVotes { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<UpVotes> UserUpVotes { get; set; } = new List<UpVotes>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
