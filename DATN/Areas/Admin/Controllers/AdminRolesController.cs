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
    public class AdminRolesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public AdminRolesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminRoles
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View(await _context.Role.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Admin/AdminRoles/Create
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // POST: Admin/AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleName,Description")] Role role)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (ModelState.IsValid)
                    {
                        var checkRoleExits = _context.Role.AsNoTracking().FirstOrDefault(r => r.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower());
                        if (checkRoleExits != null)
                        {
                            TempData["name"] = "Không đặt được trùng tên vài trò";
                            return View(role);
                        }
                        _context.Add(role);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    _notyfService.Error("Có lỗi xảy ra");
                    return View(role);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var role = await _context.Role.FindAsync(id);
                    if (role == null)
                    {
                        return NotFound();
                    }
                    return View(role);
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        // POST: Admin/AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,Description")] Role role)
        {

            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != role.RoleId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var checkRoleExits = _context.Role.AsNoTracking().FirstOrDefault(r => r.RoleName.Trim().ToLower() == role.RoleName.Trim().ToLower() && r.RoleId != id);
                            if (checkRoleExits != null)
                            {
                                TempData["name"] = "Không đặt được trùng tên vài trò";
                                return View(role);
                            }
                            _context.Update(role);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Thay đổi thành công");
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!RoleExists(role.RoleId))
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
                    return View(role);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var role = await _context.Role
                        .FirstOrDefaultAsync(m => m.RoleId == id);
                    if (role == null)
                    {
                        return NotFound();
                    }

                    return View(role);
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: Admin/AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                if (Convert.ToInt32(roleID) != 1)
                {
                    _notyfService.Error("Chức năng này chỉ dành cho quản trị viên cấp cao");
                    return RedirectToAction("Index", "Home");
                }
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var checkAccounts = _context.Account.AsNoTracking().FirstOrDefault(a => a.RoleId == id);
                    if (checkAccounts != null)
                    {
                        _notyfService.Error("Vẫn còn tài khoản thuộc vai trò này trong hệ thống");
                        return RedirectToAction(nameof(Index));

                    }
                    var role = await _context.Role.FindAsync(id);
                    _context.Role.Remove(role);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa vai trò thành công");
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.RoleId == id);
        }
    }
}
