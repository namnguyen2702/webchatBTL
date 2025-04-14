using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webchatBTL.Models;

namespace webchatBTL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // ✅ Ghi cookie "LastVisited" với giá trị là thời gian hiện tại
        CookieOptions option = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1) // cookie sống trong 1 ngày
        };

        Response.Cookies.Append("LastVisited", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), option);
        return View();
    }
    public IActionResult ViewLastVisit()
    {
        // ✅ Đọc cookie "LastVisited"
        string lastVisit = Request.Cookies["LastVisited"];

        if (string.IsNullOrEmpty(lastVisit))
        {
            lastVisit = "Bạn chưa từng truy cập trước đây!";
        }

        ViewBag.LastVisited = lastVisit;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}