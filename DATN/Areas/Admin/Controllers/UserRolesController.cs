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
    public class UserRolesController : Controller
    {
        private readonly EndProjectContext _context;

        public UserRolesController(EndProjectContext context)
        {
            _context = context;
        }

        // GET: Admin/UserRoles
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View(await _context.Role.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/UserRoles/Details/5
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

        // GET: Admin/UserRoles/Create
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

        // POST: Admin/UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,Description")] Role role)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(role);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(role);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/UserRoles/Edit/5
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

        // POST: Admin/UserRoles/Edit/5
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
                            _context.Update(role);
                            await _context.SaveChangesAsync();
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

        // GET: Admin/UserRoles/Delete/5
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

        // POST: Admin/UserRoles/Delete/5
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
                    var role = await _context.Role.FindAsync(id);
                    _context.Role.Remove(role);
                    await _context.SaveChangesAsync();
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
