using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Extension;
using DATN.Helpper;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public AdminAccountsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminAccounts
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
                    ViewData["quyentruycap"] = new SelectList(_context.Role, "RoleName", "Description");

                    List<SelectListItem> isTrangThai = new List<SelectListItem>();
                    isTrangThai.Add(new SelectListItem() { Text = "Block", Value = "0" });
                    isTrangThai.Add(new SelectListItem() { Text = "Active", Value = "0" });
                    ViewData["isTrangThai"] = isTrangThai;

                    var dbMarketContext = _context.Account.Include(a => a.Role).Where(a => a.RoleId != 3);
                    return View(await dbMarketContext.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
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

                    var account = await _context.Account
                        .FirstOrDefaultAsync(m => m.AccountId == id);
                    if (account == null)
                    {
                        return NotFound();
                    }

                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminAccounts/Create
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
                    ViewData["Role"] = new SelectList(_context.Role, "RoleId", "RoleName");
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId, Password,Email,FullName,BirthDay,Address,Phone,CreatedDate, RoleId")] Account account, Microsoft.AspNetCore.Http.IFormFile fAvatar)
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
                        if (!Utlities.IsValidEmail(account.Email))
                        {
                            TempData["errorEmail"] = "Email không hợp lệ";
                            return View(account);
                        }

                        if (!Utlities.IsInteger(account.Phone) || account.Phone.Length > 12 || account.Phone.Length < 9)
                        {
                            TempData["errorPhone"] = "Số điện thoại không hợp lệ, gồm số có độ dài từ 9-12";
                            return View(account);
                        }

                        if (!Utlities.IsValidPass(account.Password))
                        {
                            TempData["errorPass"] = "Mật khẩu phải dài tối thiểu 8 ký tự và chứa ít nhất một chữ hoa, một chữ thường, một ký tự đặc biệt và không chứa khoảng trắng";
                            return View(account);
                        }

                        var checkInfor = _context.Account.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == account.Email);
                        if (checkInfor != null)
                        {
                            TempData["errorEmail"] = "Email này đã được sử dụng";
                            return View(account);
                        }

                        checkInfor = _context.Account.AsNoTracking().SingleOrDefault(x => x.Phone.Trim() == account.Phone);
                        if (checkInfor != null)
                        {
                            TempData["errorPhone"] = "Phone này đã được sử dụng";
                            return View(account);
                        }


                        string salt = Utlities.GetRandomKey();
                        account.Email = account.Email.Trim().ToLower();
                        account.Phone = account.Phone.Trim().ToLower();
                        account.Password = (account.Password + salt.Trim()).ToMD5();
                        account.Salt = salt;
                        account.RoleId = account.RoleId;
                        account.Lock = false;
                        account.CreatedDate = DateTime.Now;

                        if (fAvatar != null)
                        {
                            string extension = Path.GetExtension(fAvatar.FileName);
                            string image = Utlities.SEOUrl(account.Email.Trim()) + extension;
                            account.Avatar = await Utlities.UploadFile(fAvatar, @"AccountAvatars", image.ToLower());
                        }
                        else
                        {
                            account.Avatar = "default-avatar.png";
                        }

                        _context.Add(account);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Tạo tài khoản thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    _notyfService.Error("Tạo tài khoản thất bại");
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminAccounts/Edit/5
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

                    var account = await _context.Account.FindAsync(id);
                    if (account == null)
                    {
                        return NotFound();
                    }
                    ViewData["Role"] = new SelectList(_context.Role, "RoleId", "RoleName");
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FullName,BirthDay,Address,Phone, RoleId")] Account account, Microsoft.AspNetCore.Http.IFormFile fAvatar)
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
                    if (id != account.AccountId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {

                            if (!Utlities.IsInteger(account.Phone) || account.Phone.Length > 12 || account.Phone.Length < 9)
                            {
                                TempData["errorPhone"] = "Số điện thoại không hợp lệ, gồm số có độ dài từ 9-12";
                                return View(account);
                            }
                            var accountOld = await _context.Account.FindAsync(id);

                            var checkInfor = _context.Account.AsNoTracking().SingleOrDefault(x => x.Phone.Trim() == account.Phone);

                            if (checkInfor != null && checkInfor.AccountId != id)
                            {
                                TempData["errorPhone"] = "Phone này đã được sử dụng";
                                return View(account);
                            }
                            accountOld.FullName = account.FullName;
                            accountOld.BirthDay = account.BirthDay;
                            accountOld.Phone = account.Phone;
                            accountOld.Address = account.Address;
                            accountOld.RoleId = account.RoleId;

                            if (fAvatar != null)
                            {
                                string extension = Path.GetExtension(fAvatar.FileName);
                                string image = Utlities.SEOUrl(accountOld.Email.Trim()) + extension;
                                accountOld.Avatar = await Utlities.UploadFile(fAvatar, @"AccountAvatars", image.ToLower());
                            }

                            _context.Update(accountOld);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Thay đổi thành công");

                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!AccountExists(account.AccountId))
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
                    _notyfService.Success("Thay đổi thất bại");
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        public async Task<IActionResult> Blocked(int? id)
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

                    var account = await _context.Account
                        .Include(a => a.Role)
                        .FirstOrDefaultAsync(m => m.AccountId == id);
                    if (account == null)
                    {
                        _notyfService.Error("Không tìm thấy tài khoản");
                        return RedirectToAction(nameof(Index));
                    }

                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/UserAccounts/Delete/5
        [HttpPost, ActionName("Blocked")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockedConfirmed(int id)
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
                    var account = await _context.Account
                                                .Include(a => a.Role)
                                                .FirstOrDefaultAsync(m => m.AccountId == id && m.Lock == false);
                    if (account == null)
                    {
                        _notyfService.Error("Tài khoản này đã bị khóa");
                    }
                    else
                    {
                        account.Lock = true;
                        _context.Account.Update(account);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Blocked thành công tài khoản");

                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        [HttpPost, ActionName("UnBlocked")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBlockedConfirmed(int id)
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
                    var account = await _context.Account
                                                .Include(a => a.Role)
                                                .FirstOrDefaultAsync(m => m.AccountId == id && m.Lock == true);
                    if (account == null)
                    {
                        _notyfService.Error("Tài khoản này không bị khóa");
                    }
                    else
                    {
                        account.Lock = false;
                        _context.Account.Update(account);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Mở khóa thành công tài khoản");

                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountId == id);
        }
    }
}
