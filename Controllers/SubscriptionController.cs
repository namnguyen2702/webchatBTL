using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace BTL.Controllers
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
<<<<<<< HEAD
                _notyfService.Warning("Không xác định được công ty của bạn. Vui lòng kiểm tra lại.");
=======
                _notyfService.Warning("Không xác định được công ty của bạn. Vui lòng đăng nhập lại.");
>>>>>>> origin/develop
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
            // Lấy thông tin user hiện tại từ Claims
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int companyId = int.Parse(User.FindFirstValue("CompanyId"));

            // Kiểm tra xem user có vai trò là "Manager" không
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == userId && u.CompanyId == companyId);

            if (user == null || user.Role.RoleName != "Manager")
            {
                _notyfService.Warning("Bạn không có quyền đăng ký. Hãy liên hệ quản lý.");
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra nếu gói dịch vụ đã tồn tại và hợp lệ
            var plan = _context.SubscriptionPlans.FirstOrDefault(p => p.PlanId == planId);
            if (plan == null)
            {
                _notyfService.Error("Gói dịch vụ không tồn tại.");
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra xem công ty đã đăng ký gói này chưa và gói này có còn hiệu lực không
            var existingSubscription = _context.CompanySubscriptions
                .FirstOrDefault(cs => cs.CompanyId == companyId && cs.PlanId == planId && cs.EndDate >= DateTime.Now);

            if (existingSubscription != null)
            {
                _notyfService.Information("Công ty đã đăng ký gói dịch vụ này và vẫn còn hiệu lực.");
                return RedirectToAction("Index", "ChatHub");
            }

            // Thêm gói đăng ký mới cho công ty
            var newSubscription = new CompanySubscription
            {
                CompanyId = companyId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1) // Giả sử gói đăng ký có thời hạn 1 tháng
            };

            _context.CompanySubscriptions.Add(newSubscription);
            _context.SaveChanges();

            _notyfService.Success("Đăng ký dịch vụ thành công.");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
