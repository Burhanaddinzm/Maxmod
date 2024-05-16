namespace Maxmod.Areas.Admin.ViewModels.Category;

public class UpdateCategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IFormFile? Image { get; set; }
    public int? ParentId { get; set; }

    public static implicit operator UpdateCategoryVM(Models.Category category)
    {
        return new UpdateCategoryVM
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId
        };
    }
}
