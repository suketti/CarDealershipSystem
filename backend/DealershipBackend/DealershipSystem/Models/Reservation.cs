namespace DealershipSystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int CarId { get; set; }
    public DateTime Date { get; set; }
}