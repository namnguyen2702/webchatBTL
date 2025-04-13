using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using webchatBTL.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace webchatBTL.Controllers
{
    [Authorize]
    [Route("register-service")]
    public class ServiceRegistrationController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        private readonly INotyfService _notyf;

        public ServiceRegistrationController(WebchatBTLDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            int companyId = int.Parse(User.FindFirstValue("CompanyId") ?? "0");

            var liveChatPlan = _context.SubscriptionPlans.FirstOrDefault(p => p.PlanName == "LiveChat");
            if (liveChatPlan == null)
            {
                _notyf.Error("Không tìm thấy gói dịch vụ LiveChat.");
                return RedirectToAction("Index", "Home");
            }

            var hasRegistered = _context.CompanySubscriptions.Any(cs =>
                cs.CompanyId == companyId &&
                cs.PlanId == liveChatPlan.PlanId &&
                cs.EndDate >= DateTime.Now);

            ViewBag.HasRegistered = hasRegistered;
            ViewBag.Plan = liveChatPlan;

            return View();
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe()
        {
            int companyId = int.Parse(User.FindFirstValue("CompanyId") ?? "0");
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId && u.CompanyId == companyId);
            if (user == null || user.Role?.RoleName != "Manager")
            {
                _notyf.Warning("Chỉ quản lý mới có thể đăng ký dịch vụ.");
                return RedirectToAction("Index");
            }

            var plan = _context.SubscriptionPlans.FirstOrDefault(p => p.PlanName == "LiveChat");
            if (plan == null)
            {
                _notyf.Error("Không tìm thấy gói LiveChat.");
                return RedirectToAction("Index");
            }

            // Kiểm tra đã có chưa
            var existing = _context.CompanySubscriptions.FirstOrDefault(cs =>
                cs.CompanyId == companyId &&
                cs.PlanId == plan.PlanId &&
                cs.EndDate >= DateTime.Now);

            if (existing != null)
            {
                _notyf.Information("Công ty đã đăng ký dịch vụ này.");
                return RedirectToAction("Index", "ChatHub");
            }

            _context.CompanySubscriptions.Add(new CompanySubscription
            {
                CompanyId = companyId,
                PlanId = plan.PlanId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            });

            _context.SaveChanges();
            _notyf.Success("Đăng ký thành công!");

            return RedirectToAction("Index", "ChatHub");
        }
    }
}
