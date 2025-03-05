namespace DealershipSystem.Interfaces;

public interface IMessageService
{
    Task CreateMessageAsync(string content, Guid recipient);
    Task<List<Message>> GetMessagesByUserAsync(Guid userId);
}
