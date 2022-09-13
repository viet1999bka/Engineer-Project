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
    public class LanguagesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public LanguagesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Languages
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View(await _context.Language.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Languages/Details/5
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

                    var language = await _context.Language
                        .FirstOrDefaultAsync(m => m.LanguageId == id);
                    if (language == null)
                    {
                        return NotFound();
                    }

                    return View(language);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Languages/Create
        public IActionResult Create()
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
                        return View();
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

        // POST: Admin/Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LanguageName,LạnguageDisplay,LanguageExten,LanguageMode,Version")] Language language)
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
                            var checkName = _context.Language.AsNoTracking().FirstOrDefault(x => x.LanguageName == language.LanguageName);
                            if (checkName != null)
                            {
                                TempData["errorName"] = "Ngôn ngữ này đã tồn tại";
                                ViewBag.languageName = language.LanguageName;
                                return View(language);

                            }
                            _context.Add(language);
                            _notyfService.Success("Thêm thành công");
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        return View(language);
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

        // GET: Admin/Languages/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }

                    var language = await _context.Language.FindAsync(id);
                    if (language == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                    return View(language);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LanguageId,LanguageName,LạnguageDisplay,LanguageExten,LanguageMode,Version")] Language language)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != language.LanguageId)
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var checkName = _context.Language.AsNoTracking().FirstOrDefault(x => x.LanguageName == language.LanguageName && x.LanguageId != id);
                            if (checkName != null)
                            {
                                TempData["errorName"] = "Ngôn ngữ này đã tồn tại";
                                return View(language);
                            }
                            _context.Update(language);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!LanguageExists(language.LanguageId))
                            {
                                _notyfService.Error("Có lỗi xảy ra");
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _notyfService.Error("Có lỗi xảy ra");
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        _notyfService.Success("Chỉnh sửa thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    return View(language);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Languages/Delete/5
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
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }

                    var language = await _context.Language
                        .FirstOrDefaultAsync(m => m.LanguageId == id);
                    if (language == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }

                    var langSup = _context.LanguageSupport.AsNoTracking().FirstOrDefault(x => x.LanguageId == id);
                    if (langSup != null)
                    {
                        _notyfService.Error("Có bài tập đang hỗ trợ ngôn ngữ này! Không thể xóa");
                        return RedirectToAction(nameof(Index));
                    }

                    return View(language);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Languages/Delete/5
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
                    if (id == 0)
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                    var language = await _context.Language.FindAsync(id);

                    var langSup = _context.LanguageSupport.AsNoTracking().FirstOrDefault(x => x.LanguageId == id);
                    if (langSup != null)
                    {
                        _notyfService.Error("Có bài tập đang hỗ trợ ngôn ngữ này! Không thể xóa");
                        return RedirectToAction(nameof(Index));
                    }
                    _context.Language.Remove(language);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa thành công");
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool LanguageExists(int id)
        {
            return _context.Language.Any(e => e.LanguageId == id);
        }
    }
}
