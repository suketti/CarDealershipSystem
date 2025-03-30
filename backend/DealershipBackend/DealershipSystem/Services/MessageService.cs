using System.Diagnostics;
using System.Security.Claims;
using DealershipSystem.Context;
using DealershipSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Service for managing messages within the dealership system.
/// </summary>
public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageService"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public MessageService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Creates a new message asynchronously.
    /// </summary>
    /// <param name="content">The content of the message.</param>
    /// <param name="recipient">The recipient's ID.</param>
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

    /// <summary>
    /// Gets messages by user ID asynchronously.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>A list of messages for the specified user.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not authorized to view the messages.</exception>
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

    /// <summary>
    /// Gets the user ID from the JWT.
    /// </summary>
    /// <returns>The user ID as a Guid.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user ID is not found in the JWT.</exception>
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

    /// <summary>
    /// Deletes a message asynchronously.
    /// </summary>
    /// <param name="messageId">The ID of the message to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the message is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not authorized to delete the message.</exception>
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