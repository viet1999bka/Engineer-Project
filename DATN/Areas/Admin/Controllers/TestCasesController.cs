using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestCasesController : Controller
    {
        private readonly EndProjectContext _context;

        public INotyfService _notyfService { get; }

        public TestCasesController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/TestCases
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

                    List<TestCase> lsTestCase = new List<TestCase>();


                    if (ExerciseID != 0)
                    {
                        lsTestCase = _context.TestCase.AsNoTracking().Where(x => x.ExerciseId == ExerciseID)
                                            .Include(t => t.Exercise).Include(c => c.Exercise.Challenge).OrderBy(x => x.Status).ToList();
                    }
                    else
                    {
                        lsTestCase = _context.TestCase.AsNoTracking()
                                            .Include(t => t.Exercise).Include(c => c.Exercise.Challenge).ToList();
                    }

                    ViewData["exercise"] = new SelectList(_context.Exercise, "ExerciseID".ToString(), "ExerciseName", ExerciseID);
                    PagedList<TestCase> models = new PagedList<TestCase>(lsTestCase.AsQueryable(), pageNumber, pageSize);
                    ViewBag.CurrentExerciseId = ExerciseID;
                    ViewBag.CurrentPage = pageNumber;
                    return View(models);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        public IActionResult Filtter(int TestId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var url = $"/Admin/Exercises?TestId={TestId}";
                    if (TestId == 0)
                    {
                        url = $"/Admin/Exercises";
                    }
                    return Json(new { status = "success", redirectUrl = url });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }


        // GET: Admin/TestCases/Create
        public IActionResult Create(int ExerciseId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var exercise = _context.Exercise.AsNoTracking().Include(e => e.Challenge).FirstOrDefault(e => e.ExcerciseId == ExerciseId);
                    if (exercise == null) return RedirectToAction("Index", "Exercises");
                    List<SelectListItem> isTrangThai = new List<SelectListItem>();
                    isTrangThai.Add(new SelectListItem() { Text = "Ẩn", Value = "True" });
                    isTrangThai.Add(new SelectListItem() { Text = "Hiện", Value = "False" });
                    ViewData["isTrangThai"] = isTrangThai;
                    ViewData["exerciseId"] = exercise.ExcerciseId;
                    ViewData["exerciseName"] = exercise.ExcerciseName.Trim();
                    ViewData["challengeName"] = exercise.Challenge.ChallengeName.Trim();
                    return View();
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/TestCases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestId,ExerciseId,Status,Input,Output")] TestCase testCase)
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
                        _context.Add(testCase);
                        await _context.SaveChangesAsync();
                        _notyfService.Success("Thêm thành công test case");
                        return RedirectToAction(nameof(Index), new { ExerciseID = testCase.ExerciseId });
                    }
                    var exercise = _context.Exercise.AsNoTracking().Include(e => e.Challenge).FirstOrDefault(e => e.ExcerciseId == testCase.ExerciseId);
                    if (exercise == null) return RedirectToAction("Index", "Exercises");
                    List<SelectListItem> isTrangThai = new List<SelectListItem>();
                    isTrangThai.Add(new SelectListItem() { Text = "Ẩn", Value = "True" });
                    isTrangThai.Add(new SelectListItem() { Text = "Hiện", Value = "False" });
                    ViewData["isTrangThai"] = isTrangThai;
                    ViewData["exerciseId"] = exercise.ExcerciseId;
                    ViewData["exerciseName"] = exercise.ExcerciseName.Trim();
                    ViewData["challengeName"] = exercise.Challenge.ChallengeName.Trim();
                    return View(testCase);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/TestCases/Edit/5
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

                    var testCase = await _context.TestCase.Include(e => e.Exercise).Include(c => c.Exercise.Challenge).FirstAsync(t => t.TestId == id);
                    if (testCase == null)
                    {
                        return NotFound();
                    }
                    List<SelectListItem> isTrangThai = new List<SelectListItem>();
                    isTrangThai.Add(new SelectListItem() { Text = "Ẩn", Value = "True" });
                    isTrangThai.Add(new SelectListItem() { Text = "Hiện", Value = "False" });
                    ViewData["isTrangThai"] = isTrangThai;
                    ViewData["chanllengeName"] = testCase.Exercise.Challenge.ChallengeName;
                    ViewData["exerciseName"] = testCase.Exercise.ExcerciseName;
                    return View(testCase);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/TestCases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,ExerciseId,Status,Input,Output")] TestCase testCase)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (id != testCase.TestId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(testCase);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!TestCaseExists(testCase.TestId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        _notyfService.Success("Chỉnh sửa thành công");
                        return RedirectToAction(nameof(Index), new { ExerciseID = testCase.ExerciseId });
                    }
                    ViewData["ExerciseId"] = new SelectList(_context.Exercise, "ExcerciseId", "ExcerciseId", testCase.ExerciseId);
                    return View(testCase);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // GET: Admin/TestCases/Delete/5
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

                    var testCase = await _context.TestCase
                        .Include(t => t.Exercise)
                        .Include(c => c.Exercise.Challenge)
                        .FirstOrDefaultAsync(m => m.TestId == id);
                    if (testCase == null)
                    {
                        return NotFound();
                    }

                    return View(testCase);
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        // POST: Admin/TestCases/Delete/5
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
                    var testCase = await _context.TestCase.FindAsync(id);
                    _context.TestCase.Remove(testCase);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Xóa thành công");
                    return RedirectToAction(nameof(Index), new { ExerciseID = testCase.ExerciseId });
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        private bool TestCaseExists(int id)
        {
            return _context.TestCase.Any(e => e.TestId == id);
        }
    }
}
