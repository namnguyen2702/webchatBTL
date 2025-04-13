using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using webchatBTL.Models;

namespace webchatBTL.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("File")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly WebchatBTLDbContext _context;

        public FileController(IWebHostEnvironment env, WebchatBTLDbContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0) return Json(new { success = false, message = "Không có file" });

            if (file.Length > 10 * 1024 * 1024) return Json(new { success = false, message = "File quá lớn (>10MB)" });

            var folderPath = Path.Combine(_env.WebRootPath, "uploads", "files");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Json(new { success = true, filePath = $"/uploads/files/{fileName}", fileName = file.FileName });
        }
    }

}