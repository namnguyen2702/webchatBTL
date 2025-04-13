using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using webchatBTL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Collections.Generic;

namespace webchatBTL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly WebchatBTLDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public ChatHub(WebchatBTLDbContext context, IHttpContextAccessor accessor, IWebHostEnvironment env)
        {
            _context = context;
            _httpContextAccessor = accessor;
            _env = env;
        }

        public override async System.Threading.Tasks.Task OnConnectedAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }

            await base.OnConnectedAsync();
        }

        public async System.Threading.Tasks.Task SendMessage(int receiverId, string message, string filePath, string fileName)
        {
            var senderId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sender = await _context.Users.FindAsync(senderId);

            var msg = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.Now,
                IsRead = true
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            object fileInfo = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                var file = new Models.File
                {
                    FileName = fileName,
                    FilePath = filePath,
                    UploadedAt = DateTime.Now,
                    UploadedBy = senderId,
                    MessageId = msg.MessageId
                };

                _context.Files.Add(file);
                await _context.SaveChangesAsync();

                fileInfo = new
                {
                    name = fileName,
                    url = filePath
                };
            }

            // Gửi realtime
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, sender.FullName, message, msg.SentAt, fileInfo);
            await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", senderId, sender.FullName, message, msg.SentAt, fileInfo);
        }




    }
}
