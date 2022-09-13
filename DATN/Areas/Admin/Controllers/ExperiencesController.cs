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
    public class ExperiencesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public ExperiencesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Experiences
        public async Task<IActionResult> Index(int? id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false && a.Lock == false);
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
                    var endProjectContext = _context.Experience.Include(e => e.User).Where(x => x.UserId == id);
                    return View(await endProjectContext.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Experiences/Details/5
        public async Task<IActionResult> Details(int? userId, int? experienceId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || experienceId == null)
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

                    var experience = await _context.Experience
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(m => m.ExperienceId == experienceId && m.UserId == userId);
                    if (experience == null)
                    {
                        _notyfService.Error("Dữ liệu không tồn tại!");
                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    return View(experience);
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // GET: Admin/Experiences/Create
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
                        _notyfService.Error("Không có dữ liệu");
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

        // POST: Admin/Experiences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("UserId,Company,JobTitle,JobLocation,StartDate,EndDate")] Experience experience)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != experience.UserId)
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

                        _context.Add(experience);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Thêm kinh nghiệm làm việc thành công");
                        ViewData["user"] = checkId.Email.Trim();
                        return RedirectToAction("Index", new { id = id });
                    }
                    ViewData["userID"] = id;
                    return View(experience);
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // GET: Admin/Experiences/Edit/5
        public async Task<IActionResult> Edit(int? userId, int? experienceId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || experienceId == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                    if (checkId == null)
                    {

                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    var experience = await _context.Experience.AsNoTracking().Where(e => e.ExperienceId == experienceId).FirstOrDefaultAsync();
                    if (experience == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "Experiences", new { id = userId });
                    }
                    ViewData["user"] = checkId.Email.Trim();
                    ViewData["userID"] = userId;
                    return View(experience);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Experiences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, int experienceId, [Bind("ExperienceId,UserId,Company,JobTitle,JobLocation,StartDate,EndDate")] Experience experience)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (experienceId != experience.ExperienceId || userId != experience.UserId)
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

                            _context.Update(experience);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Chỉnh sửa thành công");
                            ViewData["user"] = checkId.Email.Trim();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ExperienceExists(experience.ExperienceId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction("Index", "Experiences", new { id = userId });

                    }
                    ViewData["UserId"] = new SelectList(_context.Account, "AccountId", "AccountId", experience.UserId);
                    return View(experience);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Experiences/Delete/5
        public async Task<IActionResult> Delete(int? userId, int? experienceId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (userId == null || experienceId == null)
                    {
                        _notyfService.Error("Lỗi id không hợp lệ!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    var checkId = _context.Account.AsNoTracking().FirstOrDefault(a => a.AccountId == userId && a.RoleId == 3);
                    if (checkId == null)
                    {

                        return RedirectToAction("Details", "UserAccounts", new { id = userId });
                    }

                    var experience = await _context.Experience
                        .Include(e => e.User)
                        .FirstOrDefaultAsync(m => m.ExperienceId == experienceId && m.UserId == userId);
                    if (experience == null)
                    {
                        _notyfService.Error("Xảy ra lỗi!");
                        return RedirectToAction("Index", "UserAccounts");
                    }

                    return View(experience);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Experiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int experienceId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    try
                    {
                        var experience = await _context.Experience.AsNoTracking().Where(e => e.ExperienceId == experienceId && e.UserId == userId).FirstAsync();
                        _context.Experience.Remove(experience);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Xóa thành công");

                        return RedirectToAction("Index", "Experiences", new { id = userId });
                    }
                    catch
                    {
                        _notyfService.Error("Có lỗi xảy ra");

                        return RedirectToAction("Index", "Experiences", new { id = userId });
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        private bool ExperienceExists(int id)
        {
            return _context.Experience.Any(e => e.ExperienceId == id);
        }
    }
}
