using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EducationsController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public EducationsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Educations
        public async Task<IActionResult> Index(int? id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id == null)
                    {
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == id && a.RoleId == 3);
                    if (checkId == null)
                    {
                        return RedirectToAction("Details", "UserAccounts", new { id = id });
                    }

                    ViewData["user"] = checkId.Email.Trim();
                    ViewData["userID"] = id;
                    var endProjectContext = _context.Education.AsNoTracking().Include(e => e.User).Where(x => x.UserId == id);
                    return View(await endProjectContext.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // GET: Admin/Educations/Details/5
        public async Task<IActionResult> Details(int? userId, int? educationId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || educationId == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                    if (checkId == null)
                    {
                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }
                    ViewData["user"] = checkId.Email.Trim();

                    var education = await _context.Education
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(m => m.EducationId == educationId);
                    if (education == null)
                    {
                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    return View(education);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Educations/Create
        public IActionResult Create(int? id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id == null)
                    {
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == id && a.RoleId == 3);
                    if (checkId == null)
                    {
                        return RedirectToAction("Details", "UserAccounts", new { id = id });
                    }
                    ViewData["user"] = checkId.Email.Trim();
                    ViewData["userID"] = id;
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // POST: Admin/Educations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("UserId,University,Degree,Subject,StartDate,EndDate")] Education education)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != education.UserId)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }
                    if (ModelState.IsValid)
                    {
                        var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == id && a.RoleId == 3);
                        if (checkId == null)
                        {

                            return RedirectToAction("Details", "UserAccounts", new { id = id });
                        }
                        _context.Add(education);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Thêm học vấn thành công");
                        ViewData["user"] = checkId.Email.Trim();
                        return RedirectToAction("Index", new { id = id });
                    }
                    ViewData["UserId"] = new SelectList(_context.Account, "AccountId", "AccountId", education.UserId);
                    return View(education);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Educations/Edit/5
        public async Task<IActionResult> Edit(int? userId, int? educationId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || educationId == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                    if (checkId == null)
                    {

                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    var education = await _context.Education.FindAsync(educationId);
                    if (education == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "Educations", new { id = userId });
                    }

                    ViewData["user"] = checkId.Email.Trim();
                    ViewData["userID"] = userId;
                    return View(education);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Educations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, int educationId, [Bind("EducationId,UserId,University,Degree,Subject,StartDate,EndDate")] Education education)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (educationId != education.EducationId || userId != education.UserId)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                            if (checkId == null)
                            {

                                return RedirectToAction("Details", "UserAccounts", new { id = userId });
                            }
                            _context.Update(education);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Chỉnh sửa thành công");
                            ViewData["user"] = checkId.Email.Trim();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EducationExists(education.EducationId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction("Index", "Educations", new { id = userId });
                    }
                    ViewData["UserId"] = new SelectList(_context.Account, "AccountId", "AccountId", education.UserId);
                    return View(education);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Educations/Delete/5
        public async Task<IActionResult> Delete(int? userId, int? educationId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || educationId == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                    if (checkId == null)
                    {

                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    var education = await _context.Education
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(m => m.EducationId == educationId && m.UserId == userId);
                    if (education == null)
                    {
                        _notyfService.Error("Xảy ra lỗi!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    ViewData["user"] = checkId.Email.Trim();
                    return View(education);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int educationId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var education = await _context.Education.AsNoTracking().Where(e => e.EducationId == educationId && e.UserId == userId).FirstAsync();
                    _context.Education.Remove(education);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa thành công");
                    return RedirectToAction("Index", "Educations", new { id = userId });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool EducationExists(int id)
        {
            return _context.Education.Any(e => e.EducationId == id);
        }
    }
}
