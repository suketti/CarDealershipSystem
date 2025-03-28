using System.Diagnostics;
using System.Security.Claims;
using DealershipSystem.Context;
using DealershipSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MessageService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateMessageAsync(string content, Guid recipient)
    {
        var message = new Message
        {
            Content = content,
            Date = DateTime.UtcNow, // Automatically apply the current date
            Recipient = recipient
        };

        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessagesByUserAsync(Guid userId)
    {
        var currentUserId = GetUserIdFromJwt();
        if (currentUserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to view these messages.");
        }

        return await _dbContext.Messages
            .Where(m => m.Recipient == userId)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    private Guid GetUserIdFromJwt()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Id");

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in JWT.");
        }

        // Parse and return the user ID as a Guid
        return Guid.Parse(userIdClaim.Value);
    }

    public async Task DeleteMessageAsync(int messageId)
    {
        var currentUserId = GetUserIdFromJwt();

        // Find the message to delete
        var message = await _dbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
        {
            throw new KeyNotFoundException("Message not found.");
        }

        // Check if the message belongs to the current user
        if (message.Recipient != currentUserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this message.");
        }

        // Remove the message from the database
        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    
}