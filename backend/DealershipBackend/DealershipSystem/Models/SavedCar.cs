namespace DealershipSystem.Models;

public class SavedCar
{
    public int Id { get; set; }
    public Guid UserId { get; set; } 
    public int CarId { get; set; } 
}
