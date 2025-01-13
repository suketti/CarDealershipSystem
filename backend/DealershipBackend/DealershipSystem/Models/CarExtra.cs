namespace DealershipSystem.Models;

public class CarExtra
{
    public int CarExtraID { get; set; }
    public int CarID { get; set; }
    public int ExtraID { get; set; }

    // Navigation properties
    public virtual Car Car { get; set; } = null!;
    public virtual Car Extra { get; set; } = null!;
}