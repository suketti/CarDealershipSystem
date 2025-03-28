using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    // GET api/messages/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Message>>> GetMessagesByUser(Guid userId)
    {
        try
        {
            var messages = await _messageService.GetMessagesByUserAsync(userId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("You are not authorized to view these messages.");
        }
    }
    
    [HttpDelete("{messageId}")]
    public async Task<ActionResult> DeleteMessage(int messageId)
    {
        try
        {
            // Call the delete method from the service
            await _messageService.DeleteMessageAsync(messageId);
            return NoContent();  // No content means the operation was successful
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Message not found.");
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("You do not have permission to delete this message.");
        }
    }

}