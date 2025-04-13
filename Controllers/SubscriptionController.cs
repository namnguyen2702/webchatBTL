using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace webchatBTL.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        public INotyfService _notyfService { get; }

        public SubscriptionController(WebchatBTLDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // Phương thức kiểm tra xem công ty đã đăng ký dịch vụ cụ thể chưa
        public bool IsCompanySubscribedToService(int companyId, string serviceName)
        {
            // Lấy PlanId của dịch vụ cần kiểm tra
            int planId = _context.SubscriptionPlans
                .Where(sp => sp.PlanName == serviceName)
                .Select(sp => sp.PlanId)
                .FirstOrDefault();

            if (planId == 0)
            {
                return false; // Không tìm thấy gói dịch vụ
            }

            // Kiểm tra nếu công ty có đăng ký gói dịch vụ và đăng ký còn hiệu lực
            return _context.CompanySubscriptions
                .Any(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);
        }

        // Hàm kiểm tra xem đã đăng ký dịch vụ chưa
        public IActionResult AccessServiceFeature(string serviceName, string returnUrl)
        {
            var companyClaim = User.FindFirst("CompanyId")?.Value;

            // Kiểm tra nếu claim null hoặc không phải số
            if (string.IsNullOrEmpty(companyClaim) || !int.TryParse(companyClaim, out int companyId))
            {
                _notyfService.Warning("Không xác định được công ty của bạn. Vui lòng kiểm tra lại.");
                return RedirectToAction("Login", "Account");
            }

            if (IsCompanySubscribedToService(companyId, serviceName))
            {
                return Redirect(returnUrl); // Đã đăng ký dịch vụ → chuyển tới tính năng
            }
            else
            {
                return RedirectToAction("SubscriptionNotice", new { serviceName }); // Chưa đăng ký
            }
        }


        public IActionResult SubscriptionNotice(string serviceName)
        {
            // Lấy thông tin của gói dịch vụ từ tên dịch vụ
            var plan = _context.SubscriptionPlans.FirstOrDefault(sp => sp.PlanName == serviceName);

            if (plan == null)
            {
                // Nếu không tìm thấy dịch vụ, chuyển hướng đến trang lỗi hoặc trang chủ
                return RedirectToAction("Index", "Home");
            }

            return View(plan); // Truyền thông tin gói dịch vụ cho view
        }

        // Action Subscribe - Đăng ký dịch vụ cho công ty
        [HttpPost]
        public IActionResult Subscribe(int planId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int companyId = int.Parse(User.FindFirstValue("CompanyId"));

            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
            if (user == null || user.Role.RoleName != "Manager")
            {
                _notyfService.Error("Bạn không có quyền đăng ký.");
                return RedirectToAction("Index", "Home");
            }

            var existing = _context.CompanySubscriptions
                .FirstOrDefault(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);

            if (existing != null)
            {
                _notyfService.Information("Đã đăng ký gói này.");
                return RedirectToAction("Index", "ChatHub");
            }

            _context.CompanySubscriptions.Add(new CompanySubscription
            {
                CompanyId = companyId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            });
            _context.SaveChanges();

            _notyfService.Success("Đăng ký thành công.");
            return RedirectToAction("Index", "ChatHub");
        }

        [HttpPost]
        public IActionResult SubscribeAjax(int planId)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                int companyId = int.Parse(User.FindFirstValue("CompanyId"));

                var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null || user.Role.RoleName != "Manager")
                {
                    return Json(new { success = false, message = "Bạn không có quyền đăng ký." });
                }

                var existing = _context.CompanySubscriptions
                    .FirstOrDefault(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);

                if (existing != null)
                {
                    return Json(new { success = false, message = "Đã đăng ký gói này rồi." });
                }

                _context.CompanySubscriptions.Add(new CompanySubscription
                {
                    CompanyId = companyId,
                    PlanId = planId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1)
                });
                _context.SaveChanges();

                return Json(new { success = true, message = "Đăng ký thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
