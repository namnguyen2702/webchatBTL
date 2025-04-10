using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace BTL.Controllers
{
	public class EventController : Controller
    {
        private readonly WebchatBTLDbContext _db;
        public INotyfService _notyfService { get; }
        public EventController(WebchatBTLDbContext db, INotyfService notyfService)
        {
            _db = db;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public int GetManagedGroupId(int userId)
        {
            // Truy vấn cơ sở dữ liệu để lấy GroupId của người dùng
            var user = _db.Users
                               .Where(u => u.UserId == userId)
                               .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Trả về GroupId nếu người dùng có nhóm, hoặc giá trị mặc định nếu không có
            return user.GroupId ?? -1; // Trả về -1 nếu không có nhóm
        }
        // Kiểm tra nhóm được quản lý bởi Manager
        private bool IsManagedGroup(int? groupId, string managerId)
        {
            // Giả định: Có bảng Groups để lưu thông tin nhóm và manager
            return _db.GroupInCompanies.Any(g => g.GroupId == groupId && g.ManagerId == int.Parse(managerId));
        }

        [HttpGet]
[Authorize]
public IActionResult GetEvents()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var role = User.FindFirst(ClaimTypes.Role)?.Value;

    IQueryable<Event> events;

    if (role == "Admin")
    {
        events = _db.Events;
    }
    else if (role == "Manager")
    {
        var managedGroupId = GetManagedGroupId(int.Parse(userId));
        events = managedGroupId == -1 
            ? Enumerable.Empty<Event>().AsQueryable()
            : _db.Events.Where(e => e.GroupId == managedGroupId);
    }
    else
    {
        int uid = int.Parse(userId);
        events = _db.Events.Where(e => e.UserId == uid || e.Participants.Any(p => p.UserId == uid));
    }

    var result = events.Select(e => new
{
    id = e.EventId,                         // ✅ FullCalendar yêu cầu 'id'
    title = e.Subject,                      // ✅ FullCalendar yêu cầu 'title'
    start = e.Start.ToString("s"),          // ISO format yyyy-MM-ddTHH:mm:ss
    end = e.End.HasValue ? e.End.Value.ToString("s") : null,
    description = e.Description,
    color = e.ThemeColor,
    allDay = e.IsFullDay
}).ToList();

    return Ok(result);
}

        [HttpPost]
        [Authorize]
        public IActionResult SaveEvent(Event e)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            bool status = false;

            try
            {
                using (WebchatBTLDbContext dc = new WebchatBTLDbContext())
                {
                    if (e.IsFullDay)
                    {
                        // Nếu là sự kiện cả ngày, set End = 23:59 cùng ngày Start (nếu End không hợp lệ)
                        if (!e.End.HasValue || e.End.Value.Date == e.Start.Date)
                        {
                            e.End = e.Start.Date.AddHours(23).AddMinutes(59);
                        }
                        else if (e.End.Value.Date > e.Start.Date)
                        {
                            // Nếu End vượt sang ngày hôm sau, trừ lại 1 phút để không bị lỗi hiển thị
                            e.End = e.End.Value.AddMinutes(-1);
                        }
                    }

                    if (e.EventId > 0)
                    {
                        var v = dc.Events.FirstOrDefault(a => a.EventId == e.EventId);
                        if (v == null)
                            return NotFound();

                        if (role == "User" && v.UserId != int.Parse(userId))
                            return Unauthorized();

                        if (role == "Manager" && !IsManagedGroup(e.GroupId, userId))
                            return Unauthorized();

                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                        v.GroupId = e.GroupId;
                    }
                    else
                    {
                        var newEvent = new Event
                        {
                            Subject = e.Subject,
                            Start = e.Start,
                            End = e.End,
                            Description = e.Description,
                            IsFullDay = e.IsFullDay,
                            ThemeColor = e.ThemeColor,
                            GroupId = e.GroupId
                        };

                        if (role != "Admin" && role != "Manager")
                        {
                            newEvent.UserId = int.Parse(userId); // Chỉ gán cho chính mình
                        }
                        else
                        {
                            newEvent.UserId = int.Parse(userId); // Có thể mở rộng để gán cho người khác nếu cần
                        }

                        dc.Events.Add(newEvent);
                    }

                    dc.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }

            return Json(new { status = status, message = status ? "Lưu sự kiện thành công!" : "Lưu sự kiện thất bại!" });
        }



        [HttpPost]
        public IActionResult DeleteEvent(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var status = false;
            Console.Write(id);
            var existingEvent = _db.Events.Where(a => a.EventId == id).FirstOrDefault();
            if (existingEvent == null)
            {
                status = false;
                return NotFound();
            }

            if (role == "User" && existingEvent.UserId != int.Parse(userId))
            {
                status = false;
                return Unauthorized();
            }

            if (role == "Manager" && !IsManagedGroup(existingEvent.GroupId, userId))
            {
                status = false;
                return Unauthorized();
            }
            _db.Events.Remove(existingEvent);
            _db.SaveChanges();
            status = true;

            //return Ok($"Event with ID {id} deleted.");
            return Json(new { status = status });

            //var status = false;
            //using(BTLContext dc = new BTLContext()) { 
            //    var v = dc.Events.Where(a => a.EventId == eventID).FirstOrDefault();
            //    if(v != null) {
            //        dc.Events.Remove(v);
            //        dc.SaveChanges();
            //        status = true;

            //    } 
            //}
            //return Json(new { status = status });
        }

        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            //var userId = HttpContext.Session.GetString("UserId");
            //var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return Unauthorized();  // Trả về Unauthorized nếu không có session
            }

            return Ok(new { userId, role });
        }


    }
}
