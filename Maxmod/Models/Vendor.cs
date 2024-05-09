using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Vendor : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
}
