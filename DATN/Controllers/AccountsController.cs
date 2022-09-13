using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Extension;
using DATN.Helpper;
using DATN.Models;
using DATN.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DATN.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = "frontend")]
    public class AccountsController : Controller
    {
        private readonly EndProjectContext _context;
        public INotyfService _notyfService { get; }
        public AccountsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVm taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkInfor = _context.Account.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == taikhoan.Email);
                    if (checkInfor != null)
                    {
                        TempData["errorEmail"] = "Email này đã được sử dụng";
                        return View();
                    }

                    checkInfor = _context.Account.AsNoTracking().SingleOrDefault(x => x.Phone.Trim() == taikhoan.Phone);
                    if (checkInfor != null)
                    {
                        TempData["errorPhone"] = "Phone này đã được sử dụng";
                        return View();
                    }
                    string salt = Utlities.GetRandomKey();

                    if (!Utlities.IsValidPass(taikhoan.Password))
                    {
                        TempData["errorPass"] = "Mật khẩu phải dài tối thiểu 8 ký tự và chứa ít nhất một chữ hoa, một chữ thường, một ký tự đặc biệt và không chứa khoảng trắng";
                        return View();
                    }
                    Account khachhang = new Account
                    {
                        FullName = taikhoan.FullName,
                        Phone = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Password = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Lock = false,
                        Salt = salt,
                        RoleId = 3,
                        Avatar = "default-avatar.png",
                        CreatedDate = DateTime.Now

                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        // Lưu session Mã KH
                        HttpContext.Session.SetString("CustomerId", khachhang.AccountId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        // Indentity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("CustomerId", khachhang.AccountId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync("frontend", claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]

        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utlities.IsValidEmail(customer.UserName);
                    if (!isEmail)
                    {
                        TempData["ErrorEmail"] = "Email không hợp lệ";
                        return View();
                    }

                    var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName && x.RoleId == 3);
                    if (khachhang == null)
                    {
                        TempData["ErrorEmail"] = "Sai tên đăng nhập hoặc mật khẩu";
                        return View();
                    }
                    string pass = (customer.Password + khachhang.Salt.Trim()).ToMD5();

                    if (khachhang.Password != pass)
                    {
                        TempData["ErrorPass"] = "Mật khẩu của bạn không chính xác";
                        return View();
                    }

                    // Kiểm tra xem account có bị disable không
                    if (khachhang.Lock == true)
                    {
                        TempData["ErrorEmail"] = "Tài khoản của bạn đã bị khóa";
                        return View();
                    }

                    // Lưu session MaKh
                    HttpContext.Session.SetString("CustomerId", khachhang.AccountId.ToString());
                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("CustomerId", khachhang.AccountId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync("frontend", claimsPrincipal);
                    return RedirectToAction("Dashboard", "Accounts");
                }
                else
                {
                    return RedirectToAction("DangKyTaiKhoan", "Accounts");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var records = _context.ExerciseAttemp.Where(p => p.UserId == khachhang.AccountId).Include(x => x.Exercise.Level);
                    var record = records.Where(p => p.TimeAttemp == records.Max(r => r.TimeAttemp)).FirstOrDefault();
                    var listChallenge = _context.Challenge.AsNoTracking().Take(2).ToList();
                    ViewData["listChallenge"] = listChallenge;
                    ViewBag.listExerciseAttemp = record;
                    return View(khachhang);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("dang-xuat.html", Name = "logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("frontend");
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }
    }
}
