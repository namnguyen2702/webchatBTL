using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.ModelsViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using webchatBTL.Models;

<<<<<<< HEAD
namespace webchatBTL.Controllers
=======
namespace BTL.Controllers
>>>>>>> origin/develop
{
    public class ChatHubController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        public INotyfService _notyfService { get; }

        public ChatHubController(WebchatBTLDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [Route("chat.html")]
        public IActionResult Index()
        {
            var currentUserId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(currentUserId);

            var khachhang = _context.Users.AsNoTracking()
                .SingleOrDefault(x => x.UserId == userId);

            if (khachhang != null)
            {
                ViewBag.UserProfile = khachhang;

                // Lấy danh sách các cuộc trò chuyện gần đây của người dùng hiện tại
                var recentChats = _context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .AsEnumerable()
                    .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                    .Select(g => new RecentChatModel
                    {
                        // Lấy người tham gia cuộc trò chuyện (người nhận hoặc người gửi)
                        User = _context.Users.FirstOrDefault(u => u.UserId == g.Key),
                        // Lấy tin nhắn cuối cùng của cuộc trò chuyện
                        LastMessage = g.OrderByDescending(m => m.SentAt).FirstOrDefault(),
                        // Đếm số tin nhắn chưa đọc từ người dùng hiện tại
                        UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                    })
                    .ToList();

                // Lấy danh sách các nhóm mà người dùng tham gia
                var userGroups = _context.GroupMembers
                    .Where(gm => gm.UserId == userId)
                    .Select(gm => new GroupModel
                    {
                        GroupName = gm.Group.GroupName,
                        UnreadCount = _context.Messages
                            .Where(m => m.GroupId == gm.GroupId && m.SenderId != userId && !m.IsRead)
                            .Count() // Đếm số lượng tin nhắn chưa đọc trong group
                    })
                    .ToList();

                var viewModel = new ChatSidebarViewModel
                {
                    RecentChats = recentChats,
                    Groups = userGroups
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //tìm user: những user đã chat cùng
        [HttpGet]
        public IActionResult SearchUsers(string searchQuery)
        {
            try
            {
                var currentUserId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Json(new { success = false, message = "User not logged in" });
                }

                int userId = Convert.ToInt32(currentUserId);

                // Nếu input rỗng thì hiện tất cả cuộc trò chuyện
                if (string.IsNullOrEmpty(searchQuery)) {
                    var users = _context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .AsEnumerable()
                    .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                    .Select(g => new RecentChatModel
                    {
                        // Lấy người tham gia cuộc trò chuyện (người nhận hoặc người gửi)
                        User = _context.Users.FirstOrDefault(u => u.UserId == g.Key),
                        // Lấy tin nhắn cuối cùng của cuộc trò chuyện
                        LastMessage = g.OrderByDescending(m => m.SentAt)
                                        .Select(m => new Message
                                        {
                                            Content = m.Content,
                                            SentAt = m.SentAt
                                        })
                                        .FirstOrDefault(),
                        // Đếm số tin nhắn chưa đọc từ người dùng hiện tại
                        UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                    })
                    .ToList();

                    return Json(new { success = true, data = users });
                }
                else
                {
                    var users = _context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .AsEnumerable() // Đưa dữ liệu vào bộ nhớ
                    .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
                    .Select(g => new RecentChatModel
                    {
                        User = _context.Users.FirstOrDefault(u => u.UserId == g.Key && u.FullName.ToLower().Contains(searchQuery.ToLower())),
                        LastMessage = g.OrderByDescending(m => m.SentAt)
                                        .Select(m => new Message
                                        {
                                            Content = m.Content,
                                            SentAt = m.SentAt
                                        })
                                        .FirstOrDefault(),
                        UnreadCount = g.Count(m => m.ReceiverId == userId && !m.IsRead)
                    })
                    .Where(g => g.User != null) // Lọc những user hợp lệ
                    .ToList();

                    return Json(new { success = true, data = users });
                } 
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc trả về lỗi chi tiết
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public IActionResult SearchGroups(string searchQueryGroup)
        {
            try
            {
                var currentUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var groupsQuery = _context.Groups
                            .Where(g => _context.GroupMembers.Any(gm => gm.GroupId == g.GroupId && gm.UserId == currentUserId));

                //k rong
                if (!string.IsNullOrEmpty(searchQueryGroup))
                {
                    groupsQuery = groupsQuery
                        .Where(g => g.GroupName.ToLower().Contains(searchQueryGroup.ToLower()));
                }

                var groups = groupsQuery
                    .Select(g => new
                    {
                        g.GroupId,
                        g.GroupName
                    })
                    .ToList();

                return Json(new { success = true, groups });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //searchuser: tìm tất cả user 
        public IActionResult SearchAllUsers(string searchAllUser)
        {
            try
            {
                if(string.IsNullOrEmpty(searchAllUser))
                {
                    return Json(new { success = false, data = "" });
                }    

                // Lấy tất cả người dùng từ database
                var users = _context.Users
                    .Where(u => u.FullName.ToLower().Contains(searchAllUser.ToLower()))
                    .OrderBy(u => u.FullName)
                    .Select(u => new
                    {
                        u.UserId,
                        u.FullName,
                        FirstLetter = u.FullName.Substring(0, 1).ToUpper() // Lấy ký tự đầu tiên để nhóm theo ký tự
                    })
                    .ToList();

                // Nhóm người dùng theo chữ cái đầu tiên của tên
                var groupedUsers = users
                    .GroupBy(u => u.FirstLetter)
                    .ToDictionary(g => g.Key, g => g.ToList());

                return Json(new { success = true, data = groupedUsers });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
