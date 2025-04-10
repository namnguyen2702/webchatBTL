using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using webchatBTL.Models;

[Authorize]
public class ChatHub : Hub
{
    private readonly WebchatBTLDbContext _context;

    public ChatHub(WebchatBTLDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task SendMessage(int receiverId, string message)
    {
        var senderId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var senderName = Context.User.Identity.Name;

        // Lưu vào DB
        var msg = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            SentAt = DateTime.Now,
            IsRead = false
        };
        _context.Messages.Add(msg);
        await _context.SaveChangesAsync();

        // Gửi cho người nhận
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderName, message);

        // Gửi lại cho người gửi (hiển thị phía mình)
        await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderName, message);
    }
}
