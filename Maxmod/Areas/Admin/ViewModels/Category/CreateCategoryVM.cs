namespace Maxmod.Areas.Admin.ViewModels.Category;

public class CreateCategoryVM
{
    public string Name { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
    public int? ParentId { get; set; }
}
