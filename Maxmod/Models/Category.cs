using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public List<Category>? SubCategories { get; set; }
    public List<Product>? Products { get; set; }
    public Category()
    {
        SubCategories = new List<Category>();
    }
}
