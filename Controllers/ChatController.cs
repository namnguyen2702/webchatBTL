using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;

namespace webchatBTL.Controllers
{
    [Authorize]
public class ChatController : Controller
{
    private readonly WebchatBTLDbContext _context;

    public ChatController(WebchatBTLDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserList()
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var users = await _context.Users
            .Where(u => u.UserId != currentUserId)
            .Select(u => new { u.UserId, u.FullName })
            .ToListAsync();
        return Json(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetChatHistory(int receiverId)
    {
        var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var messages = await _context.Messages
            .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                        (m.SenderId == receiverId && m.ReceiverId == senderId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return Json(messages);
    }
}

}