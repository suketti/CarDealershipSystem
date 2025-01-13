namespace DealershipSystem.Models;

public class Color
{
    public int ID { get; set; } // Primary key
    public string ColorNameEnglish { get; set; } = string.Empty!; // Color name in English
    public string ColorNameJapanese { get; set; } = string.Empty!; // Color name in Japanese

    // Navigation property
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}