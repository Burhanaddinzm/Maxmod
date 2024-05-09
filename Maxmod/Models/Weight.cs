using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Weight : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public List<ProductWeight> ProductWeights { get; set; }
    public Weight()
    {
        ProductWeights = new List<ProductWeight>();
    }
}
