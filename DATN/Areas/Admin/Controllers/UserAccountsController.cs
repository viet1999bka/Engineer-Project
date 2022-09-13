using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Extension;
using DATN.Helpper;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserAccountsController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public UserAccountsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefaultAsync(x => x.Phone.ToLower() == Phone);
                if (khachhang == null)
                    return Json(data: "Số điện thoại : " + Phone + " Đã được sử dụng<br />");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        [HttpGet]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefaultAsync(x => x.Email.ToLower() == Email);
                if (khachhang == null)
                    return Json(data: "Email : " + Email + " Đã được sử dụng<br />");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        // GET: Admin/UserAccounts
        public async Task<IActionResult> Index(int page = 1, int status = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var pageNumber = page;
                    var pageSize = 10;
                    List<Account> lsAccounts = new List<Account>();
                    if (status != 0)
                    {

                        bool lockVal = (status != 2) ? true : false;
                        lsAccounts = _context.Account.AsNoTracking()
                            .Where(x => x.RoleId == 3 && x.Lock == lockVal)
                            .OrderByDescending(x => x.AccountId).ToList();
                    }
                    else
                    {
                        lsAccounts = _context.Account.AsNoTracking()
                            .Where(x => x.RoleId == 3)
                            .OrderByDescending(x => x.AccountId).ToList();
                    }

                    List<SelectListItem> lsTrangThai = new List<SelectListItem>();
                    lsTrangThai.Add(new SelectListItem() { Text = "Tất cả", Value = "0" });
                    lsTrangThai.Add(new SelectListItem() { Text = "Hoạt động", Value = "2" });
                    lsTrangThai.Add(new SelectListItem() { Text = "Bị khóa", Value = "1" });
                    ViewBag.lsTrangThai = lsTrangThai;
                    ViewBag.CurrentStatus = status;

                    PagedList<Account> models = new PagedList<Account>(lsAccounts.AsQueryable(), pageNumber, pageSize);
                    ViewBag.CurrentPage = pageNumber;
                    return View(models);
                }
            }

            return RedirectToAction("Login", "Accounts");

        }


        public IActionResult Fitler(int status = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var url = $"/Admin/UserAccounts?status={status}";
                    if (status == 0)
                    {
                        url = $"/Admin/UserAccounts";
                    }

                    return Json(new { status = "success", redirectUrl = url });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/UserAccounts/Details/5
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

                    var account = await _context.Account
                        .Include(a => a.Role)
                        .FirstOrDefaultAsync(m => m.AccountId == id && m.RoleId == 3);
                    if (account == null)
                    {
                        return NotFound();
                    }

                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }



        // GET: Admin/UserAccounts/Create
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

        // POST: Admin/UserAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Email,Password,FullName,BirthDay,Address,Phone,Admin,NationId,RoleId,Lock,Active,LastLogin,CreatedDate")] Account account, Microsoft.AspNetCore.Http.IFormFile fAvatar)
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
                        account.RoleId = 3;
                        account.Lock = false;
                        account.CreatedDate = DateTime.Now;
                        account.Avatar = "default-avatar.png";

                        if (fAvatar != null)
                        {
                            string extension = Path.GetExtension(fAvatar.FileName);
                            string image = account.Email.Trim().ToLower() + extension;
                            account.Avatar = await Utlities.UploadFile(fAvatar, @"AccountAvatars", image);
                        }

                        _context.Add(account);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Thêm thành công người dùng");
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleId", account.RoleId);
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/UserAccounts/Edit/5
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

                    var account = await _context.Account.FindAsync(id);
                    if (account == null)
                    {
                        return NotFound();
                    }
                    ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "RoleId", account.RoleId);
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FullName,BirthDay,Address,Phone")] Account account, Microsoft.AspNetCore.Http.IFormFile fAvatar)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
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

                            if (fAvatar != null)
                            {
                                string extension = Path.GetExtension(fAvatar.FileName);
                                string image = account.Email.Trim().ToLower() + extension;
                                accountOld.Avatar = await Utlities.UploadFile(fAvatar, @"AccountAvatars", image);
                            }

                            _context.Update(accountOld);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!AccountExists(id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        _notyfService.Success("Thay đổi thông tin tài khoản thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    return View(account);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/UserAccounts/Delete/5
        public async Task<IActionResult> Blocked(int? id)
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

                    var account = await _context.Account
                        .Include(a => a.Role)
                        .FirstOrDefaultAsync(m => m.AccountId == id && m.RoleId == 3);
                    if (account == null)
                    {
                        return NotFound();
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
