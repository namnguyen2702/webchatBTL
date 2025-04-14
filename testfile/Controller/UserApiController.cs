// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Mvc;
// using webchatBTL.Models;

// [Microsoft.AspNetCore.Components.Route("api/[controller]")]
// [ApiController]
// public class UserApiController : ControllerBase
// {
//     private readonly WebchatBTLDbContext _context;
//     public UserApiController(WebchatBTLDbContext context)
//     {
//         _context = context;
//     }

//     [HttpGet]
//     public IActionResult GetAll()
//     {
//         var users = _context.Users.Select(u => new
//         {
//             u.UserId,
//             u.FullName,
//             u.Email,
//             u.VerifiKey
//         }).ToList();

//         return Ok(users);
//     }
// }