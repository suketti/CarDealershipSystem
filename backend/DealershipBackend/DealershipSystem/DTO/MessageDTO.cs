namespace DealershipSystem.DTO;

public class MessageDTO
{
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public Guid Recipient { get; set; }
}