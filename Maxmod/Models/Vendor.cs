using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Vendor : BaseAuditableEntity
{
    public string Name { get; set; } = "Vendor_" + Guid.NewGuid().ToString();
    public string Image { get; set; } = "2_1c4b30d7-c150-41db-8703-2e4d065c8cbe.png";
}
