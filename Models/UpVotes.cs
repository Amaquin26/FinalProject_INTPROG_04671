using System.ComponentModel.DataAnnotations;

namespace FashionWebsite.Models
{
    public class UpVotes
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
        public int DesignId { get; set; }   
        public Design Design { get; set; }

    }
}
