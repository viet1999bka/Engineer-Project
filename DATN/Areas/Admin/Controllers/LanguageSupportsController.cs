using AspNetCoreHero.ToastNotification.Abstractions;
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
    public class LanguageSupportsController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public LanguageSupportsController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/LanguageSupports
        public async Task<IActionResult> Index(int page = 1, int ExerciseID = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var pageNumber = page;
                    var pageSize = 20;

                    List<LanguageSupport> lsLanguage = new List<LanguageSupport>();

                    if (ExerciseID != 0)
                    {

                        lsLanguage = _context.LanguageSupport.AsNoTracking().Include(e => e.Exercise).Include(e => e.Exercise.Challenge).Include(e => e.Language)
                            .Where(x => x.ExerciseId == ExerciseID).ToList();
                    }
                    else
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction("Index", "Exercises");
                    }

                    PagedList<LanguageSupport> models = new PagedList<LanguageSupport>(lsLanguage.AsQueryable(), pageNumber, pageSize);
                    ViewBag.CurrentPage = pageNumber;
                    ViewBag.CurrentExerciseId = ExerciseID;
                    return View(models);
                }
            }
            return RedirectToAction("Login", "Accounts");


        }

        public IActionResult Filtter(int ChallengeId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var url = $"/Admin/LanguageSupports?ChallengeId={ChallengeId}";
                    if (ChallengeId == 0)
                    {
                        url = $"/Admin/LanguageSupports";

                    }
                    return Json(new { status = "success", redirectUrl = url });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/LanguageSupports/Details/5
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

                    var languageSupport = await _context.LanguageSupport
                        .Include(l => l.Exercise)
                        .Include(l => l.Exercise.Challenge)
                        .Include(l => l.Language)
                        .FirstOrDefaultAsync(m => m.LanguageSupId == id);
                    if (languageSupport == null)
                    {
                        return NotFound();
                    }

                    if (languageSupport.FileMain != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", languageSupport.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + languageSupport.Exercise.ExcerciseName.Trim().ToLower());
                        path += "\\" + languageSupport.FileMain.Trim();
                        ViewData["pathMain"] = path;
                    }

                    if (languageSupport.FileFunction != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", languageSupport.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + languageSupport.Exercise.ExcerciseName.Trim().ToLower());
                        path += "\\" + languageSupport.FileFunction.Trim();
                        ViewData["pathFunc"] = path;
                    }

                    ViewData["languageSupId"] = id;
                    return View(languageSupport);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/LanguageSupports/Create
        public IActionResult Create(int ExerciseId)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (ExerciseId != 0)
                    {
                        var exercise = _context.Exercise.AsNoTracking().Include(e => e.Challenge).SingleOrDefault(e => e.ExcerciseId == ExerciseId);
                        ViewData["ExerciseId"] = ExerciseId;
                        ViewData["ExerciseName"] = exercise.ExcerciseName.Trim();
                        ViewData["ChallengeName"] = exercise.Challenge.ChallengeName.Trim();
                        ViewData["LanguageId"] = new SelectList(_context.Language, "LanguageId", "LanguageName");
                        return View();
                    }
                    return RedirectToAction("Index", new { ExerciseID = ExerciseId });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/LanguageSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ExerciseId, [Bind("ExerciseId,LanguageId,FileMain,FileFunction, Description")] LanguageSupport languageSupport, Microsoft.AspNetCore.Http.IFormFile fMain, Microsoft.AspNetCore.Http.IFormFile fFuntion)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (ExerciseId == 0)
                    {
                        _notyfService.Error("Lỗi hệ thống");
                        return RedirectToAction("Index", "Exercises");
                    }

                    var exercise = _context.Exercise.AsNoTracking().Include(e => e.Challenge).SingleOrDefault(e => e.ExcerciseId == ExerciseId);

                    if (ModelState.IsValid)
                    {
                        var checkLanguageExits = _context.LanguageSupport.AsNoTracking().FirstOrDefault(l => l.ExerciseId == ExerciseId && l.LanguageId == languageSupport.LanguageId);
                        if (checkLanguageExits != null)
                        {
                            TempData["Language"] = "Bài tập đã hỗ trợ ngôn ngữ này";
                        }
                        else
                        {
                            if (fMain != null)
                            {
                                string file = fMain.FileName;
                                languageSupport.FileMain = await Utlities.UploadFileCode(fMain, @"" + exercise.Challenge.ChallengeName.Trim().ToLower() + "/" + exercise.ExcerciseName.Trim().ToLower(), file);
                            }

                            if (fFuntion != null)
                            {
                                string file = fFuntion.FileName;
                                languageSupport.FileFunction = await Utlities.UploadFileCode(fFuntion, @"" + exercise.Challenge.ChallengeName.Trim().ToLower() + "/" + exercise.ExcerciseName.Trim().ToLower(), file);
                            }
                            _context.Add(languageSupport);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Thêm thành công");
                            return RedirectToAction(nameof(Index), new { ExerciseID = ExerciseId });
                        }

                    }



                    ViewData["ExerciseId"] = ExerciseId;
                    ViewData["ExerciseName"] = exercise.ExcerciseName.Trim();
                    ViewData["ChallengeName"] = exercise.Challenge.ChallengeName.Trim();
                    ViewData["LanguageId"] = new SelectList(_context.Language, "LanguageId", "LanguageName");

                    return View(languageSupport);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/LanguageSupports/Edit/5
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

                    var languageSupport = await _context.LanguageSupport.AsNoTracking()
                                                .Include(e => e.Exercise).Include(c => c.Exercise.Challenge)
                                                .Include(l => l.Language).FirstOrDefaultAsync(l => l.LanguageSupId == id);
                    if (languageSupport == null)
                    {
                        return NotFound();
                    }
                    ViewData["LanguageId"] = new SelectList(_context.Language, "LanguageId", "LanguageName");
                    ViewData["challenge"] = languageSupport.Exercise.Challenge.ChallengeName;
                    ViewData["exercise"] = languageSupport.Exercise.ExcerciseName;
                    return View(languageSupport);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/LanguageSupports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LanguageSupId,ExerciseId,LanguageId, Description")] LanguageSupport languageSupport, string challengeName, string exerciseName, Microsoft.AspNetCore.Http.IFormFile fMain, Microsoft.AspNetCore.Http.IFormFile fFuntion)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != languageSupport.LanguageSupId)
                    {
                        return NotFound();
                    }
                    ViewData["LanguageId"] = new SelectList(_context.Language, "LanguageId", "LanguageName");
                    ViewData["challenge"] = challengeName;
                    ViewData["exercise"] = exerciseName;

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            // Kiểm tra tồn tại của ngôn ngữ mới
                            var checkLanguage = _context.LanguageSupport.AsNoTracking().FirstOrDefault(l => l.LanguageId == languageSupport.LanguageId && l.LanguageSupId != id && l.ExerciseId == languageSupport.ExerciseId); ;
                            if (checkLanguage != null)
                            {
                                TempData["Language"] = "Bài tập đã hỗ trợ ngôn ngữ trên";

                                return View(languageSupport);
                            }

                            var oldLanguage = _context.LanguageSupport.AsNoTracking().Include(e => e.Exercise)
                                .Include(e => e.Exercise.Challenge)
                                .FirstOrDefault(l => l.LanguageSupId == id);

                            // Kiểm tra xem có đổi từ ngôn ngữ này sang ngôn ngữ khác không
                            if (oldLanguage.LanguageId != languageSupport.LanguageId)
                            {
                                // Nếu có thì xóa đi file code của ngôn ngữ cũ
                                if (oldLanguage.FileMain != null)
                                {
                                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower());
                                    path += "\\" + oldLanguage.FileMain;
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                    oldLanguage.FileMain = null;
                                }

                                if (oldLanguage.FileFunction != null)
                                {
                                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower());
                                    path += "\\" + oldLanguage.FileFunction;
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                    oldLanguage.FileFunction = null;
                                }
                            }

                            if (fMain != null)
                            {
                                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower());
                                path += "\\" + oldLanguage.FileMain;
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                string file = fMain.FileName;
                                oldLanguage.FileMain = await Utlities.UploadFileCode(fMain, @"" + oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "/" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower(), file);
                            }

                            if (fFuntion != null)
                            {
                                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower());
                                path += "\\" + oldLanguage.FileFunction;
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                string file = fFuntion.FileName;
                                oldLanguage.FileFunction = await Utlities.UploadFileCode(fFuntion, @"" + oldLanguage.Exercise.Challenge.ChallengeName.Trim().ToLower() + "/" + oldLanguage.Exercise.ExcerciseName.Trim().ToLower(), file);
                            }



                            oldLanguage.LanguageId = languageSupport.LanguageId;
                            oldLanguage.Description = languageSupport.Description;
                            _context.LanguageSupport.Update(oldLanguage);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Thay đổi thành công");
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!LanguageSupportExists(languageSupport.LanguageSupId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(Index), new { ExerciseID = languageSupport.ExerciseId });
                    }

                    return View(languageSupport);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/LanguageSupports/Delete/5
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

                    var languageSupport = await _context.LanguageSupport
                        .Include(l => l.Exercise)
                        .Include(e => e.Exercise.Challenge)
                        .Include(l => l.Language)
                        .FirstOrDefaultAsync(m => m.LanguageSupId == id);
                    if (languageSupport == null)
                    {
                        return NotFound();
                    }

                    return View(languageSupport);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/LanguageSupports/Delete/5
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
                    var languageSupport = await _context.LanguageSupport.AsNoTracking()
                                           .Include(l => l.Exercise).Include(l => l.Exercise.Challenge)
                                           .FirstOrDefaultAsync(l => l.LanguageSupId == id);
                    // Xóa file main
                    if (languageSupport.FileMain != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", languageSupport.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + languageSupport.Exercise.ExcerciseName.Trim().ToLower());
                        path += "\\" + languageSupport.FileMain.Trim();
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    // xóa file code
                    if (languageSupport.FileFunction != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", languageSupport.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + languageSupport.Exercise.ExcerciseName.Trim().ToLower());
                        path += "\\" + languageSupport.FileFunction.Trim();
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }



                    _context.LanguageSupport.Remove(languageSupport);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa thành công");
                    return RedirectToAction(nameof(Index), new { ExerciseID = languageSupport.ExerciseId });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        [HttpGet]
        [Route("xemcode.html", Name = "ViewFile")]
        public async Task<IActionResult> ViewFile(int id, string path, int kindFile)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    ViewData["contentFile"] = null;

                    if (System.IO.File.Exists(path))
                    {
                        string content = System.IO.File.ReadAllText(path);
                        ViewData["contentFile"] = content;
                    }

                    ViewData["languageSupId"] = id;
                    ViewData["path"] = path;
                    ViewData["kindFile"] = kindFile;

                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        public async Task<IActionResult> deleteFile(int id, string path, int kindFile)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                            var languageSup = _context.LanguageSupport.AsNoTracking().FirstOrDefault(l => l.LanguageSupId == id);
                            if (kindFile == 0)
                            {
                                languageSup.FileMain = null;
                            }
                            else
                            {
                                languageSup.FileFunction = null;
                            }
                            _context.LanguageSupport.Update(languageSup);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Xóa file thành công");
                            return RedirectToAction("Details", new { id = id });
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    _notyfService.Error("Khôn tìm thấy file. Thử lại sau");
                    return RedirectToAction("ViewFile", new { id = id, path = path });
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        private bool LanguageSupportExists(int id)
        {
            return _context.LanguageSupport.Any(e => e.LanguageSupId == id);
        }
    }
}
