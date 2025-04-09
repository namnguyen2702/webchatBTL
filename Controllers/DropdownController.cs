using webchatBTL.Models;
//using Google;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class DropdownController : ControllerBase
{
    private readonly WebchatBTLDbContext _context;

    public DropdownController(WebchatBTLDbContext context)
    {
        _context = context;
    }

    // API trả về danh sách người dùng
    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _context.Users.Select(u => new { u.UserId, u.FullName}).ToList();
        return Ok(users);
    }

    // API trả về danh sách nhóm
    [HttpGet("groupInCompanies")]
    public IActionResult GetGroups()
    {
        //var groups = _context.GroupInCompanies.Select(g => new { g.GroupId, g.GroupName }).ToList();
        var groups = _context.GroupInCompanies.ToList();
        return Ok(groups);
    }
}
