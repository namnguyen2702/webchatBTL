using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webchatBTL.Models;
using Microsoft.AspNetCore.Authorization;

namespace webchatBTL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCompanyController : Controller
    {
        private readonly WebchatBTLDbContext _context;

        public AdminCompanyController(WebchatBTLDbContext context)
        {
            _context = context;
        }

        // GET: AdminCompany
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies
                .Include(c => c.CompanySubscriptions)
                .ThenInclude(cs => cs.Plan) // Lấy cả thông tin Subscription Plan cho mỗi công ty
                .ToListAsync()

            );
        }

        // GET: AdminCompany/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: AdminCompany/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCompany/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,Domain,CreatedAt")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: AdminCompany/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: AdminCompany/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CompanyName,Domain,CreatedAt")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Subscribe/5
        public IActionResult Subscribe(int id)
        {
            var company = _context.Companies.Find(id);
            if (company == null) return NotFound();

            ViewBag.SubscriptionPlans = _context.SubscriptionPlans.ToList();
            return View(new CompanySubscription { CompanyId = id });
        }

        // POST: Company/Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe(CompanySubscription subscription)
        {
            if (ModelState.IsValid)
            {
                subscription.StartDate = DateTime.Now;
                subscription.EndDate = DateTime.Now.AddMonths(1); // thuê bao 1 tháng (có thể tùy chỉnh)

                _context.CompanySubscriptions.Add(subscription);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = subscription.CompanyId });
            }

            ViewBag.SubscriptionPlans = _context.SubscriptionPlans.ToList();
            return View(subscription);
        }


        // GET: AdminCompany/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: AdminCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
