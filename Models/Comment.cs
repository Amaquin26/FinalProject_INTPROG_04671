using System.ComponentModel.DataAnnotations;

namespace FashionWebsite.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int DesignId { get; set; }
        public Design Design { get; set; }  
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
