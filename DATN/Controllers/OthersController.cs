using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Controllers
{
    public class OthersController : Controller
    {
        private readonly EndProjectContext _context;
        private readonly INotyfService _notyf;

        public OthersController(EndProjectContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Others

        [HttpGet]
        [AllowAnonymous]
        [Route("View-Profile", Name = "Index")]
        public async Task<IActionResult> Index(int? id)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    if (id != null)
                    {
                        if (id == khachhang.AccountId) return RedirectToAction("Index", "UserProfile");
                        var other = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == id && x.RoleId == 3);
                        if (other != null)
                        {
                            var lsExperience = await _context.Experience.AsNoTracking().Where(t => t.UserId == id).Take(5).OrderBy(x => x.ExperienceId).ToListAsync();
                            var lsEducation = await _context.Education.AsNoTracking().Where(t => t.UserId == id).Take(5).OrderBy(x => x.EducationId).ToListAsync();
                            var lsExercise = await _context.ExerciseAttemp.AsNoTracking().Include(e => e.Exercise).Where(t => t.UserId == id).Take(5).OrderBy(x => x.TimeAttemp).ToListAsync();
                            var lsCodeFriend = await _context.CodeFriend.AsNoTracking().Include(e => e.Friend).Where(t => t.UserId == id && t.Status == 1).Take(5).ToListAsync();
                            var checkFriend = _context.CodeFriend.AsNoTracking().Include(e => e.Friend).SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id);
                            if (checkFriend != null)
                            {
                                ViewData["Friend"] = checkFriend.Status;
                                lsCodeFriend.Remove(checkFriend);
                            }
                            else
                            {
                                ViewData["Friend"] = 3;

                            }
                            ViewData["lsExperience"] = lsExperience;
                            ViewData["lsEducation"] = lsEducation;
                            ViewData["lsExercise"] = lsExercise;
                            ViewData["lsCodeFriend"] = lsCodeFriend;
                            return View(other);
                        }

                    }
                    _notyf.Error("Tài khoản này không tồn tại");
                    return RedirectToAction("Index", "UserProfile");

                }

            }
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("ListFriendOfOther", Name = "OtherCodeFriend")]
        public async Task<IActionResult> OtherCodeFriend(int id = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var listUserFriend = _context.CodeFriend.AsNoTracking().Where(u => u.UserId == khachhang.AccountId).ToList();
                    var listOtherFriend = await _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == id && u.Status == 1).ToListAsync();

                    ViewData["otherId"] = id;
                    ViewData["listUserFriend"] = listUserFriend;
                    return View(listOtherFriend);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
