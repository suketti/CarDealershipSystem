namespace DealershipSystem.DTO;

public class CarExtraDTO
{
    public int CarExtraID { get; set; }
    public int CarID { get; set; }
    public int ExtraID { get; set; }

    // Navigation property DTOs (Optional if required for nested data transfer)
    public virtual CarDTO Car { get; set; } = null!;
    public virtual ExtraDTO Extra { get; set; } = null!;
}