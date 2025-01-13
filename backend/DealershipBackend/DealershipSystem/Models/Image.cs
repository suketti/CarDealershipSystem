namespace DealershipSystem.Models;

public class Image
{
    public int ID { get; set; }
    public int CarID { get; set; }
    public string URL { get; set; } = string.Empty!;

    // Navigation properties
    public virtual Car Car { get; set; } = null!;
}
