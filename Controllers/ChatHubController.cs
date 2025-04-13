using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using webchatBTL.Models;
using webchatBTL.ModelsViews;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Globalization;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace webchatBTL.Controllers
{
    [Authorize]
    public class ChatHubController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly INotyfService _notyfService;

        public ChatHubController(WebchatBTLDbContext context, IHttpContextAccessor http, INotyfService notyfService)
        {
            _context = context;
            _http = http;
            _notyfService = notyfService;
        }

        [Route("chat.html")]
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _notyfService.Error("Không xác định được người dùng.");
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.Find(userId);
            if (user == null)
            {
                _notyfService.Error("Không tìm thấy người dùng.");
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserProfile = user;
            return View();
        }

        [HttpGet]
        public IActionResult GetFriendsInGroup()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userGroupIds = _context.GroupMembers
                .Where(g => g.UserId == userId)
                .Select(g => g.GroupId)
                .ToList();

            var friends = _context.GroupMembers
                .Where(g => userGroupIds.Contains(g.GroupId) && g.UserId != userId)
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

        [HttpGet]
        public IActionResult GetMessages(int receiverId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var messages = _context.Messages
                .Include(m => m.Files)
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    isMe = m.SenderId == currentUserId,
                    sender = _context.Users.FirstOrDefault(u => u.UserId == m.SenderId).FullName,
                    message = m.Content,
                    time = m.SentAt,
                    filePath = m.Files.FirstOrDefault().FilePath,
                    fileName = m.Files.FirstOrDefault().FileName
                }).ToList();

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "File không hợp lệ." });

                if (file.Length > 10 * 1024 * 1024)
                    return Json(new { success = false, message = "File vượt quá 10MB." });

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "files");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Json(new
                {
                    success = true,
                    filePath = "/uploads/files/" + fileName,
                    fileName = file.FileName
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

}
