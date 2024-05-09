namespace Maxmod.Models.Common;

public class BaseAuditableEntity : BaseEntity
{
    public string IP { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
