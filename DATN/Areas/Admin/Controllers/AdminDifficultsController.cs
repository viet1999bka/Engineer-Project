using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDifficultsController : Controller
    {
        private readonly EndProjectContext _context;
        public INotyfService _notyfService { get; }

        public AdminDifficultsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDifficults
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View(await _context.Difficult.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminDifficults/Details/5
        public async Task<IActionResult> Details(int? id)
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
                        return NotFound();
                    }

                    var difficult = await _context.Difficult
                        .FirstOrDefaultAsync(m => m.DifficultId == id);
                    if (difficult == null)
                    {
                        return NotFound();
                    }

                    return View(difficult);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminDifficults/Create
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminDifficults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DifficultName, Description")] Difficult difficult)
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
                        if (ModelState.IsValid)
                        {
                            var checkName = _context.Difficult.AsNoTracking().FirstOrDefault(x => x.DifficultName.Trim().ToLower() == difficult.DifficultName.ToLower());
                            if (checkName != null)
                            {
                                TempData["errorName"] = "Tên độ khó này đã được sử dụng. Không thể đặt trùng!";
                                return View(difficult);
                            }
                            _context.Add(difficult);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Tạo mới thành công");
                            return RedirectToAction(nameof(Index));
                        }
                        return View(difficult);
                    }
                    catch
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminDifficults/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                        if (id == null)
                        {
                            _notyfService.Error("Không tồn tại độ khó này!");
                            return RedirectToAction(nameof(Index));
                        }

                        var difficult = await _context.Difficult.FindAsync(id);
                        if (difficult == null)
                        {
                            _notyfService.Error("Không tồn tại độ khó này!");
                            return RedirectToAction(nameof(Index));
                        }
                        return View(difficult);
                    }
                    catch
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminDifficults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DifficultId,DifficultName,Description")] Difficult difficult)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != difficult.DifficultId)
                    {
                        _notyfService.Error("Không tồn tại độ khó này!");
                        return RedirectToAction(nameof(Index));
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var checkName = _context.Difficult.AsNoTracking().FirstOrDefault(x => x.DifficultName.Trim().ToLower() == difficult.DifficultName.ToLower() && x.DifficultId != difficult.DifficultId);
                            if (checkName != null)
                            {
                                TempData["errorName"] = "Tên độ khó này đã được sử dụng. Không thể đặt trùng!";
                                return View(difficult);

                            }
                            _context.Update(difficult);
                            _notyfService.Success("Cập nhật thành công");
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!DifficultExists(difficult.DifficultId))
                            {
                                _notyfService.Error("Không tồn tại độ khó này!");
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _notyfService.Error("Có lỗi xảy ra!");
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    return View(difficult);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminDifficults/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
                        _notyfService.Error("Có lỗi xảy ra!");
                        return RedirectToAction(nameof(Index));
                    }

                    var difficult = await _context.Difficult
                        .FirstOrDefaultAsync(m => m.DifficultId == id);
                    if (difficult == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra!");
                        return RedirectToAction(nameof(Index));
                    }

                    return View(difficult);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminDifficults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
                        var checkEx = _context.Exercise.AsNoTracking().FirstOrDefault(x => x.LevelId == id);
                        if (checkEx != null)
                        {
                            _notyfService.Error("Đang tồn tại bài tập có độ khó này. Không thể xóa!");
                            return RedirectToAction(nameof(Index));
                        }
                        var difficult = await _context.Difficult.FindAsync(id);
                        _context.Difficult.Remove(difficult);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Xóa thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        _notyfService.Error("Có lỗi xảy ra!");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool DifficultExists(int id)
        {
            return _context.Difficult.Any(e => e.DifficultId == id);
        }
    }
}
