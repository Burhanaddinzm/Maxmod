namespace Maxmod.ViewModels.Category;

public class UpdateCategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
    public int? ParentId { get; set; }
}
