using Maxmod.Models;

namespace Maxmod.Areas.Admin.ViewModels.Product
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? AdditionalImages { get; set; }
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
    }
}
