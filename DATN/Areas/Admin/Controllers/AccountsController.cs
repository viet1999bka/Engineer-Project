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

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "backend")]
    public class AccountsController : Controller
    {
        private readonly EndProjectContext _context;

        public AccountsController(EndProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/admin/dang-nhap-admin.html", Name = "DangNhapAdmin")]
        public IActionResult Login()
        {
            var taiKhoanID = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (taiKhoanID != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(taiKhoanID) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/admin/dang-nhap-admin.html", Name = "DangNhapAdmin")]

        public async Task<IActionResult> Login(LoginViewModel admin, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utlities.IsValidEmail(admin.UserName);
                    if (!isEmail)
                    {
                        TempData["ErrorEmail"] = "Email không hợp lệ";
                        return View();
                    }

                    var tkAdmin = _context.Account.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == admin.UserName && (x.RoleId == 1 || x.RoleId == 2) && x.Lock == false);
                    if (tkAdmin == null)
                    {
                        TempData["ErrorEmail"] = "Sai tên đăng nhập hoặc mật khẩu";
                        return View();
                    }
                    string pass = (admin.Password + tkAdmin.Salt.Trim()).ToMD5();

                    if (tkAdmin.Password != pass)
                    {
                        TempData["ErrorPass"] = "Mật khẩu của bạn không chính xác";
                        return View();
                    }

                    // Kiểm tra xem account có bị disable không
                    if (tkAdmin.Lock == true)
                    {
                        TempData["ErrorEmail"] = "Tài khoản của bạn đã bị khóa";
                        return View();
                    }

                    // Lưu session MaAdmin
                    HttpContext.Session.SetString("AdminId", tkAdmin.AccountId.ToString());
                    HttpContext.Session.SetString("RoleId", tkAdmin.RoleId.ToString());
                    HttpContext.Session.SetString("Avatar", tkAdmin.Avatar.ToString().Trim());

                    var taikhoanID = HttpContext.Session.GetString("CustomerId");
                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, tkAdmin.FullName),
                        new Claim("AdminId", tkAdmin.AccountId.ToString()),
                        new Claim("RoleId", tkAdmin.RoleId.ToString()),
                        new Claim("Avatar", tkAdmin.Avatar.ToString().Trim())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "loginAdmin");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync("backend", claimsPrincipal);
                    return RedirectToAction("Index", "Home");
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

        [HttpGet]
        [Route("dang-xuat-admin.html", Name = "DangXuatAdmin")]
        public IActionResult dangxuat()
        {
            HttpContext.SignOutAsync("backend");
            HttpContext.Session.Remove("AdminId");
            HttpContext.Session.Remove("RoleId");
            HttpContext.Session.Remove("Avatar");
            return RedirectToAction("Login");
        }
    }
}
