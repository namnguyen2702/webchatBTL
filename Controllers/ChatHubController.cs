using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using webchatBTL.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.Services;

namespace webchatBTL.Controllers
{
    [Authorize]
    public class ChatHubController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly ISubscriptionService _subscriptionService;



        public ChatHubController(WebchatBTLDbContext context, INotyfService notyfService, ISubscriptionService subscriptionService)
        {
            _context = context;
            _notyfService = notyfService;
            _subscriptionService = subscriptionService;
        }

        [Route("chat.html")]
        public IActionResult Index()
        {
            var companyClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyClaim) || !int.TryParse(companyClaim, out int companyId))
            {
                _notyfService.Error("Không lấy được thông tin công ty.");
                return RedirectToAction("Login", "Account");
            }

            if (!_subscriptionService.IsCompanySubscribedToService(companyId, "LiveChat"))
            {
                _notyfService.Warning("Bạn chưa đăng ký dịch vụ hoặc đã hết hạn");
                return RedirectToAction("SubscriptionNotice", "Subscription", new { serviceName = "LiveChat" });
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserProfile = _context.Users.Find(userId);

            return View();
        }


        [HttpGet("ChatHub/GetFriendsInGroup")]
        public IActionResult GetFriendsInGroup()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var groupIds = _context.GroupMembers
                .Where(g => g.UserId == userId)
                .Select(g => g.GroupId)
                .ToList();

            var friends = _context.GroupMembers
        .Include(g => g.User) // ✅ Load navigation property
        .Where(g => groupIds.Contains(g.GroupId) && g.UserId != userId)
        .Select(g => g.User)
        .Distinct()
        .Select(u => new
        {
            userId = u.UserId,
            fullName = u.FullName,
            avatarUrl = "/assets/images/users/avatar-1.jpg"
        }).ToList();

            return Json(friends);
        }

        [HttpGet("ChatHub/GetMessages")]
        public IActionResult GetMessages(int receiverId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var messages = _context.Messages
                .Include(m => m.Files)
                .Where(m =>
                    (m.SenderId == userId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == userId))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    isMe = m.SenderId == userId,
                    sender = m.Sender.FullName,
                    message = m.Content,
                    time = m.SentAt,
                    filePath = m.Files.FirstOrDefault().FilePath,
                    fileName = m.Files.FirstOrDefault().FileName
                })
                .ToList();

            return Json(messages);
        }
    }
}
