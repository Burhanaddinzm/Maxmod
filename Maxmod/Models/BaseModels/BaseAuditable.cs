namespace Maxmod.Models.BaseModels;

public class BaseAuditable : BaseModel
{
    public string IP { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
