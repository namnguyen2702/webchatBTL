using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;
using System;
using webchatBTL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using webchatBTL.ModelsViews;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;

namespace webchatBTL.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly WebchatBTLDbContext _context;
        public INotyfService _notyfService { get; }

        public AccountController(WebchatBTLDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        private string HashPassword(string password)
        {
            // Tạo salt ngẫu nhiên
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            // Dùng PBKDF2 để hash password
            var hashed = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = hashed.GetBytes(32); // 256 bit
            byte[] hashBytes = new byte[48]; // 16 salt + 32 hash
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);
            return Convert.ToBase64String(hashBytes);
        }
        private bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var hashed = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = hashed.GetBytes(32);
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }

        //public IActionResult ValidatePhone(string Phone)
        //{
        //    try
        //    {
        //        var khachhang = _context.Users.AsNoTracking()
        //                     .SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
        //        if (khachhang != null)
        //        {
        //            return Json(data: "Số điện thoại: " + Phone + " đã được sử dụng<br/>");
        //        }
        //        return Json(data: true);

        //    }
        //    catch
        //    {
        //        return Json(data: true);
        //    }
        //}

        //public IActionResult ValidateEmail(string Email)
        //{
        //    try
        //    {
        //        var khachhang = _context.Users.AsNoTracking()
        //                     .SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
        //        if (khachhang != null) {
        //            return Json(data: "Email: " + Email + " đã được sử dụng<br/>");
        //        }
        //        return Json(data: true);

        //    }
        //    catch {
        //        return Json(data: true);
        //    }
        //}


        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Users.AsNoTracking()
                    .SingleOrDefault(x => x.UserId == Convert.ToInt32(taikhoanID));
                if(khachhang != null)
                {
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> Register(RegisterViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = new User()
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone,
                        Email = taikhoan.Email,
                        PasswordHash = HashPassword(taikhoan.Password),
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        RoleId = 3
                    };
                    try
                    {
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        // Load lại để có thông tin Role
                        user = await _context.Users.Include(x => x.Role)
                        .FirstOrDefaultAsync(x => x.UserId == user.UserId);
                        
                        //lưu session khách hàng
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("UserId");

                        //Indentity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FullName),
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User"), // fallback tránh lỗi null
                            new Claim("CompanyId", user.CompanyId?.ToString() ?? "0")
                        };


                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công!");

                        return RedirectToAction("Dashboard", "Account"); 

                    }
                    catch
                    {
                        _notyfService.Error("Lỗi đăng ký: ");
                        return RedirectToAction("Register", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("UserId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel user, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Tìm người dùng trong cơ sở dữ liệu
                    var khachhang = _context.Users
                                        .AsNoTracking()
                                        .Include(x => x.Role)
                                        .FirstOrDefault(x => x.Email.Trim() == user.UserName.Trim());

                    if (khachhang == null) 
                    {
                        return RedirectToAction("Register", "Account");
                    }

                    string pass = user.Password.ToString();

                    if (!VerifyPassword(user.Password, khachhang.PasswordHash))
                    {
                        _notyfService.Error("Tài khoản hoặc mật khẩu không chính xác!");
                        return View(user);
                    }

                    //kiểm tra khách hàng có bị disable không
                    if (khachhang.IsActive == false)
                    {
                        return RedirectToAction("ThongBao", "Account");
                    }

                    //Lưu Session KH
                    HttpContext.Session.SetString("UserId", khachhang.UserId.ToString());
                    HttpContext.Session.SetString("UserRole", khachhang.Role.RoleName);  // Lưu role nếu cần
                    HttpContext.Session.SetString("UserName", khachhang.FullName);
                    var taikhoanID = HttpContext.Session.GetString("UserId");

                    //Indentity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim(ClaimTypes.NameIdentifier, khachhang.UserId.ToString()),
                        new Claim(ClaimTypes.Role, khachhang.Role.RoleName),  // Lưu Role vào Claim
                        new Claim("CompanyId", khachhang.CompanyId.ToString()) // Thêm CompanyId vào Claims
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Đăng nhập và tạo cookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    _notyfService.Success("Đăng nhập thành công!");

                    return RedirectToAction("Index", "Home");

                }
            }
            catch
            {
                return RedirectToAction("Register", "Account");
            }

            return View(user);
        }

        [HttpGet]
        [Route("dang-xuat.html", Name ="Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Index", "Home");
        }

        [Route("/hanchetruycap.html")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
