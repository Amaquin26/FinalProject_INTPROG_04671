using System.ComponentModel.DataAnnotations;

namespace FashionWebsite.ViewModels
{
    public class EditDesignViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a Design Name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a Description.")]
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Please enter a price whether it be initial or final.")]
        public decimal Price { get; set; }
    }
}
