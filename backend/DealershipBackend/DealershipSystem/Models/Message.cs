public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
    public Guid Recipient { get; set; }
}