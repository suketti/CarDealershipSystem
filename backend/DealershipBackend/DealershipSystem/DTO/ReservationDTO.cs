namespace DealershipSystem.DTO;

public class ReservationDTO
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int CarId { get; set; }
    public DateTime Date { get; set; }
}