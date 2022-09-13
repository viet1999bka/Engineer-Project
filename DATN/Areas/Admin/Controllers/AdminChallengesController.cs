using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Helpper;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminChallengesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public AdminChallengesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminChallenges
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    return View(await _context.Challenge.ToListAsync());
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminChallenges/Details/5
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

                    var challenge = await _context.Challenge
                        .FirstOrDefaultAsync(m => m.ChallengeId == id);
                    if (challenge == null)
                    {
                        return NotFound();
                    }

                    return View(challenge);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminChallenges/Create
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

        // POST: Admin/AdminChallenges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChallengeName,Description,Thumb,Title")] Challenge challenge, Microsoft.AspNetCore.Http.IFormFile fthumb)
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
                        var chaeckName = _context.Challenge.AsNoTracking().FirstOrDefault(x => x.ChallengeName.Trim().ToLower() == challenge.ChallengeName.ToLower());
                        if (chaeckName != null)
                        {
                            TempData["errorName"] = "Tên thử thách đã tồn tại không thể đặt trùng";
                            return View(challenge);

                        }
                        challenge.Thumb = "default.png";
                        if (fthumb != null)
                        {
                            string extension = Path.GetExtension(fthumb.FileName);
                            string image = challenge.ChallengeName.Trim().ToLower() + extension;
                            challenge.Thumb = await Utlities.UploadFile(fthumb, @"challenges", image);
                        }

                        challenge.CreatedDate = DateTime.Now;
                        _context.Add(challenge);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Tạo mới thành công chủ đề");
                        return RedirectToAction(nameof(Index));
                    }
                    return View(challenge);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminChallenges/Edit/5
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

                    var challenge = await _context.Challenge.FindAsync(id);
                    if (challenge == null)
                    {
                        return NotFound();
                    }
                    return View(challenge);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminChallenges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChallengeId,ChallengeName,Thumb,Description,Title")] Challenge challenge, Microsoft.AspNetCore.Http.IFormFile fthumb)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != challenge.ChallengeId)
                    {
                        _notyfService.Error("Có lỗi xảy ra thử thách không trùng khớp");
                        return RedirectToAction(nameof(Index));
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {

                            var chaeckName = _context.Challenge.AsNoTracking().FirstOrDefault(x => x.ChallengeName.Trim().ToLower() == challenge.ChallengeName.ToLower());

                            if (chaeckName != null && chaeckName.ChallengeId != id)
                            {
                                TempData["errorName"] = "Tên thử thách đã tồn tại không thể đặt trùng";
                                return View(challenge);

                            }

                            var oldChallenge = _context.Challenge.AsNoTracking().FirstOrDefault(c => c.ChallengeId == challenge.ChallengeId);
                            if (oldChallenge == null)
                            {
                                _notyfService.Error("Xảy ra lõi");
                                return RedirectToAction(nameof(Index));
                            }



                            if (fthumb != null)
                            {
                                string extension = Path.GetExtension(fthumb.FileName);
                                string image = challenge.ChallengeName.Trim().ToLower() + extension;
                                oldChallenge.Thumb = await Utlities.UploadFile(fthumb, @"challenges", image);
                            }
                            else if (chaeckName == null)
                            {
                                string oldFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", oldChallenge.ChallengeName.Trim().ToLower());


                                if (Directory.Exists(oldFolder))
                                {
                                    string newFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", challenge.ChallengeName.Trim().ToLower());
                                    Directory.Move(oldFolder, newFolder);
                                }
                                if (oldChallenge.Thumb.Trim() != "default.png")
                                {
                                    string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "challenges");
                                    string thumbOld = pathFile + "\\" + oldChallenge.Thumb.Trim();
                                    string thumbNew = thumbOld.Replace(oldChallenge.ChallengeName.Trim().ToLower(), challenge.ChallengeName.Trim().ToLower());
                                    if (System.IO.File.Exists(thumbOld)) System.IO.File.Move(thumbOld, thumbNew);
                                    oldChallenge.Thumb = oldChallenge.Thumb.Trim().Replace(oldChallenge.ChallengeName.Trim().ToLower(), challenge.ChallengeName.Trim().ToLower());
                                }
                            }

                            oldChallenge.ChallengeName = challenge.ChallengeName;
                            oldChallenge.Title = challenge.Title;
                            oldChallenge.Description = challenge.Description;
                            oldChallenge.CreatedDate = DateTime.Now;

                            _context.Update(oldChallenge);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Chỉnh sửa thành công");
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ChallengeExists(challenge.ChallengeId))
                            {
                                _notyfService.Error("Có lỗi xảy ra thử thách không trùng khớp");
                                return RedirectToAction(nameof(Index));

                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    _notyfService.Error("Có lỗi xảy ra");
                    return View(challenge);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/AdminChallenges/Delete/5
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
                        _notyfService.Error("Thử thách không tồn tại!");
                        return RedirectToAction(nameof(Index));
                    }
                    var exercise = await _context.Exercise.FirstOrDefaultAsync(m => m.ChallengeId == id);
                    var challenge = await _context.Challenge
                        .FirstOrDefaultAsync(m => m.ChallengeId == id);
                    if (challenge == null)
                    {
                        _notyfService.Error("Thử thách không tồn tại!");

                        return RedirectToAction(nameof(Index));
                    }

                    if (exercise != null)
                    {
                        _notyfService.Error("Đang có bài tập thuộc thử thách. Không thể xóa!");

                        return RedirectToAction(nameof(Index));

                    }

                    return View(challenge);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/AdminChallenges/Delete/5
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
                        var challenge = await _context.Challenge.FindAsync(id);
                        var exercise = await _context.Exercise.FirstOrDefaultAsync(m => m.ChallengeId == id);

                        if (exercise != null)
                        {
                            _notyfService.Error("Đang có bài tập thuộc thử thách. Không thể xóa!");

                            return RedirectToAction(nameof(Index));

                        }

                        if (challenge.Thumb.Trim() != "default.png")
                        {
                            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "challenges", challenge.Thumb.Trim());
                            if (System.IO.File.Exists(pathFile))
                                System.IO.File.Delete(pathFile);
                        }

                        _context.Challenge.Remove(challenge);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Xóa thành công!");
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        _notyfService.Error("Đang có bài tập thuộc thử thách. Không thể xóa!");
                        return RedirectToAction(nameof(Index));

                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenge.Any(e => e.ChallengeId == id);
        }
    }
}
