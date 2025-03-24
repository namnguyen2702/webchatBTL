using Microsoft.AspNetCore.Authorization;      // Dùng cho phân quyền truy cập (ví dụ: [Authorize], [AllowAnonymous])
using Microsoft.AspNetCore.Mvc;                // Dùng cho controller và routing
using Microsoft.AspNetCore.Mvc.Rendering;      // Dùng cho dropdown list (nếu cần)
using System.IO;                               // Dùng cho thao tác file (chưa thấy dùng trong code này)
using System.Threading.Tasks;                  // Hỗ trợ xử lý bất đồng bộ
using System;                                  // Thư viện cơ bản
using webchatBTL.Models;                              // Namespace chứa các lớp Model (User, Project, v.v.)
using Microsoft.AspNetCore.Http;               // Làm việc với Session
using System.Collections.Generic;              // Dùng cho List<T>
using System.Security.Claims;                  // Dùng cho Identity Claims
using Microsoft.AspNetCore.Authentication;     // Hỗ trợ đăng nhập với Claims
using Microsoft.EntityFrameworkCore;           // Hỗ trợ EF Core: Include(), AsNoTracking(), v.v.
using System.Linq;                             // Hỗ trợ truy vấn LINQ
using AspNetCoreHero.ToastNotification.Abstractions; // Dùng thư viện Notyf để hiện thông báo
// using webchatBTL.ModelsViews;                         // Chứa các ViewModel (LoginViewModel, RegisterViewModel)
using Microsoft.AspNetCore.Authentication.Cookies; // Dùng để xác thực cookie
[Authorize]
public class AccountController : Controller
{
    private readonly WebchatBTLDbContext _context;
    public INotyfService _notyfService{get;}

    public AccountController(WebchatBTLDbContext context, INotyfService notyfService)
    {
        _context = context;
        _notyfService = notyfService;
    }
    [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
}