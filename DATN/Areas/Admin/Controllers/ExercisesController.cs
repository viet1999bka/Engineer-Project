using AspNetCoreHero.ToastNotification.Abstractions;
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
    public class ExercisesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public ExercisesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Exercises
        public async Task<IActionResult> Index(int page = 1, int ChallengeId = 0, int LevelId = 0)
        {

            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var pageNumber = page;
                    var pageSize = 15;

                    List<Exercise> lsExercises = new List<Exercise>();


                    if (ChallengeId != 0 && LevelId != 0)
                    {
                        lsExercises = _context.Exercise.AsNoTracking().Where(x => x.ChallengeId == ChallengeId && x.LevelId == LevelId)
                                            .Include(e => e.Challenge).Include(e => e.Level).ToList();
                    }
                    else if (ChallengeId != 0 && LevelId == 0)
                    {
                        lsExercises = _context.Exercise.AsNoTracking().Where(x => x.ChallengeId == ChallengeId)
                                            .Include(e => e.Challenge).Include(e => e.Level).ToList();
                    }
                    else if (ChallengeId == 0 && LevelId != 0)
                    {
                        lsExercises = _context.Exercise.AsNoTracking().Where(x => x.LevelId == LevelId)
                                            .Include(e => e.Challenge).Include(e => e.Level).ToList();
                    }
                    else
                    {
                        lsExercises = _context.Exercise.AsNoTracking()
                                            .Include(e => e.Challenge).Include(e => e.Level).ToList();
                    }

                    ViewData["challenge"] = new SelectList(_context.Challenge, "ChallengeId".ToString(), "ChallengeName", ChallengeId);
                    ViewData["difficult"] = new SelectList(_context.Difficult, "DifficultId".ToString(), "DifficultName", LevelId);



                    //var lsExercise = _context.Exercise.AsNoTracking()
                    //    .Include(e => e.Challenge).Include(e => e.Level);

                    PagedList<Exercise> models = new PagedList<Exercise>(lsExercises.AsQueryable(), pageNumber, pageSize);
                    ViewBag.CurrentChallengeId = ChallengeId;
                    ViewBag.CurrentPage = pageNumber;
                    return View(models);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        public IActionResult Filtter(int ChallengeId = 0, int LevelId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var url = $"/Admin/Exercises?ChallengeId={ChallengeId}&LevelId={LevelId}";
                    if (ChallengeId == 0 && LevelId != 0)
                    {
                        url = $"/Admin/Exercises?LevelId={LevelId}";
                    }
                    else if (ChallengeId != 0 && LevelId == 0)
                    {
                        url = $"/Admin/Exercises?ChallengeId={ChallengeId}";

                    }
                    else if (ChallengeId == 0 && LevelId == 0)
                    {
                        url = $"/Admin/Exercises";

                    }
                    return Json(new { status = "success", redirectUrl = url });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Exercises/Details/5
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
                        _notyfService.Error("Có lỗi xảy ra");

                        return RedirectToAction(nameof(Index));
                    }

                    var exercise = await _context.Exercise
                        .Include(e => e.Challenge)
                        .Include(e => e.Level)
                        .FirstOrDefaultAsync(m => m.ExcerciseId == id);
                    if (exercise == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra");

                        return RedirectToAction(nameof(Index));
                    }

                    var languageSupport = _context.LanguageSupport.AsNoTracking().FirstOrDefault(x => x.ExerciseId == id);
                    if (languageSupport == null)
                    {
                        ViewBag.languageSupport = "Bài tập này chưa có file code thuộc bất kỳ ngôn ngữ nào. Thêm ngay!";
                    }

                    var testCase = _context.TestCase.AsNoTracking().FirstOrDefault(x => x.ExerciseId == id);
                    if (testCase == null)
                    {
                        ViewBag.testCase = "Bài tập chưa có test. Thêm ngay!";
                    }

                    return View(exercise);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Exercises/Create
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeName");
                    ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExcerciseName,LevelId,ChallengeId,Description,Example,Input,Output,TimeLimit")] Exercise exercise)
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
                            var checkName = _context.Exercise.AsNoTracking().FirstOrDefault(x => x.ExcerciseName.Trim().ToLower() == exercise.ExcerciseName.ToLower() && x.ChallengeId == exercise.ChallengeId);
                            if (checkName != null)
                            {
                                TempData["errorName"] = "Tên bài tập đã tồn tại. Không thể đặt trùng";
                                ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeName");
                                ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                                return View(exercise);
                            }

                            if (exercise.TimeLimit == null) exercise.TimeLimit = "500";
                            exercise.CreateDate = DateTime.Now;
                            _context.Add(exercise);
                            await _context.SaveChangesAsync();
                            _notyfService.Success("Tạo mới bài tập thành công");

                            return RedirectToAction(nameof(Index));
                        }
                        ViewData["ChallengeId"] = new SelectList(_context.Challenge, "ChallengeId", "ChallengeName");
                        ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                        return View(exercise);
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

        // GET: Admin/Exercises/Edit/5
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

                    var exercise = await _context.Exercise.AsNoTracking().Include(e => e.Challenge).SingleOrDefaultAsync(e => e.ExcerciseId == id);
                    if (exercise == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra");

                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                    ViewData["challengeName"] = exercise.Challenge.ChallengeName.Trim();
                    return View(exercise);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExcerciseId,ExcerciseName,ChallengeId,LevelId,Description,Example,Input,Output")] Exercise exercise, string challengeName)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != exercise.ExcerciseId)
                    {
                        _notyfService.Error("Có lỗi xảy ra");

                        return RedirectToAction(nameof(Index));
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var checkName = _context.Exercise.AsNoTracking().FirstOrDefault(x => x.ExcerciseName.Trim().ToLower() == exercise.ExcerciseName.ToLower() && x.ChallengeId == exercise.ChallengeId);
                            if (checkName != null && checkName.ExcerciseId != id)
                            {
                                TempData["errorName"] = "Tên bài tập đã tồn tại. Không thể đặt trùng";
                                ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                                ViewData["challengeName"] = challengeName;
                                return View(exercise);
                            }
                            if (checkName == null)
                            {
                                var oldName = _context.Exercise.AsNoTracking().FirstOrDefault(e => e.ExcerciseId == exercise.ExcerciseId);
                                string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", challengeName.Trim().ToLower());
                                string oldFolder = pathFolder + "\\" + oldName.ExcerciseName.Trim().ToLower();


                                if (Directory.Exists(oldFolder))
                                {
                                    string newFolder = pathFolder + "\\" + exercise.ExcerciseName.Trim().ToLower();
                                    Directory.Move(oldFolder, newFolder);
                                }
                            }
                            if (exercise.TimeLimit == null) exercise.TimeLimit = "500";

                            _context.Update(exercise);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ExerciseExists(exercise.ExcerciseId))
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
                    ViewData["LevelId"] = new SelectList(_context.Difficult, "DifficultId", "DifficultName");
                    ViewData["challengeName"] = challengeName;
                    return View(exercise);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/Exercises/Delete/5
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

                    var exercise = await _context.Exercise
                        .Include(e => e.Challenge)
                        .Include(e => e.Level)
                        .FirstOrDefaultAsync(m => m.ExcerciseId == id);
                    if (exercise == null)
                    {
                        _notyfService.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }

                    return View(exercise);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/Exercises/Delete/5
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
                        if (id == 0)
                        {
                            _notyfService.Error("Có lỗi xảy ra");
                            return RedirectToAction(nameof(Index));
                        }

                        var exercise = await _context.Exercise.AsNoTracking().Include(c => c.Challenge).FirstOrDefaultAsync(e => e.ExcerciseId == id);

                        // Xóa các test case của bài tập
                        var listTestCase = _context.TestCase.AsNoTracking()
                            .Where(x => x.ExerciseId == id).ToList();
                        if (listTestCase.Count != 0)
                        {
                            foreach (var item in listTestCase)
                            {
                                _context.TestCase.Remove(item);
                            }
                            await _context.SaveChangesAsync();
                        }

                        // Xóa đi trường những người dùng tham gia bài tập trên
                        var lisExAttemp = _context.ExerciseAttemp.AsNoTracking().Where(x => x.ExerciseId == id).ToList();
                        if (lisExAttemp.Count != 0)
                        {
                            foreach (var item in lisExAttemp)
                            {
                                _context.ExerciseAttemp.Remove(item);
                            }
                            await _context.SaveChangesAsync();
                        }

                        // Xóa ngôn ngữ hỗ trợ liên quan
                        var listLanguageSup = _context.LanguageSupport.AsNoTracking().Where(x => x.ExerciseId == id).ToList();
                        string folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + exercise.ExcerciseName.Trim().ToLower());
                        if (listLanguageSup.Count != 0)
                        {
                            foreach (var item in listLanguageSup)
                            {

                                _context.LanguageSupport.Remove(item);
                            }
                            await _context.SaveChangesAsync();
                        }
                        // Xóa folder chứa các file code
                        if (Directory.Exists(folderpath))
                        {
                            Directory.Delete(folderpath, true);
                        }





                        // Xóa đi bài tập
                        _context.Exercise.Remove(exercise);
                        await _context.SaveChangesAsync();

                        _notyfService.Success("Xóa thành công");
                        return RedirectToAction(nameof(Index));
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

        private bool ExerciseExists(int id)
        {
            return _context.Exercise.Any(e => e.ExcerciseId == id);
        }
    }
}
