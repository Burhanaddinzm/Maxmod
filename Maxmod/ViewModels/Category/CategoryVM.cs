namespace Maxmod.ViewModels.Category;

public class CategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int? ParentId { get; set; }
}
