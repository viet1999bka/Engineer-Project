using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly EndProjectContext _context;

        public SearchController(EndProjectContext context)
        {
            _context = context;
        }

        // GET : Search/FindExercise
        [HttpPost]
        public IActionResult FindExercise(string keyword, int ChallengeId = 0, int LevelId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
                    {
                        return PartialView("ListExerciseSearchPartial", null);
                    }
                    List<Exercise> ls = new List<Exercise>();
                    if (ChallengeId == 0 && LevelId != 0)
                    {
                        ls = _context.Exercise.AsNoTracking()
                        .Include(e => e.Challenge).Include(e => e.Level)
                        .Where(e => e.ExcerciseName.Contains(keyword) && e.LevelId == LevelId)
                        .OrderByDescending(e => e.ExcerciseName)
                        .Take(10)
                        .ToList();
                    }
                    else if (ChallengeId != 0 && LevelId == 0)
                    {
                        ls = _context.Exercise.AsNoTracking()
                        .Include(e => e.Challenge).Include(e => e.Level)
                        .Where(e => e.ExcerciseName.Contains(keyword) && e.ChallengeId == ChallengeId)
                        .OrderByDescending(e => e.ExcerciseName)
                        .Take(10)
                        .ToList();
                    }
                    else if (ChallengeId != 0 && LevelId != 0)
                    {
                        ls = _context.Exercise.AsNoTracking()
                        .Include(e => e.Challenge).Include(e => e.Level)
                        .Where(e => e.ExcerciseName.Contains(keyword) && e.ChallengeId == ChallengeId && e.LevelId == LevelId)
                        .OrderByDescending(e => e.ExcerciseName)
                        .Take(10)
                        .ToList();
                    }
                    else
                    {
                        ls = _context.Exercise.AsNoTracking()
                        .Include(e => e.Challenge).Include(e => e.Level)
                        .Where(e => e.ExcerciseName.Contains(keyword))
                        .OrderByDescending(e => e.ExcerciseName)
                        .Take(10)
                        .ToList();
                    }

                    if (ls == null)
                    {
                        return PartialView("ListExerciseSearchPartial", null);
                    }
                    else
                    {
                        return PartialView("ListExerciseSearchPartial", ls);
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");
            //List<Exercise> ls = new List<Exercise>();

        }

        // GET : Search/FindUser
        [HttpPost]
        public IActionResult FindUserAccount(string keyword)
        {

            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    //List<Exercise> ls = new List<Exercise>();
                    if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
                    {
                        var list = _context.Account.AsNoTracking()
                        .Where(e => e.RoleId == 3)
                        .ToList();
                        return PartialView("ListUserSearchPartial", list);
                    }
                    var ls = _context.Account.AsNoTracking()
                        .Where(e => e.Email.Contains(keyword) && e.RoleId == 3)
                        .Take(20)
                        .ToList();
                    if (ls == null)
                    {
                        return PartialView("ListUserSearchPartial", null);
                    }
                    else
                    {
                        return PartialView("ListUserSearchPartial", ls);
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        public IActionResult FindLanguage(string keyword)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
                    {
                        var list = _context.Language.AsNoTracking()
                        .ToList();
                        return PartialView("ListLanguageSearchPartial", list);
                    }
                    var ls = _context.Language.AsNoTracking()
                        .Where(e => e.LanguageName.Contains(keyword))
                        .Take(20)
                        .ToList();
                    if (ls == null)
                    {
                        return PartialView("ListLanguageSearchPartial", null);
                    }
                    else
                    {
                        return PartialView("ListLanguageSearchPartial", ls);
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

        public IActionResult FindLanguageSupport(string nameLangugae, int ExerciseId = 0)
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {

                    List<LanguageSupport> ls = new List<LanguageSupport>();
                    if (ExerciseId != 0 && nameLangugae != null)
                        ls = _context.LanguageSupport.AsNoTracking()
                         .Include(e => e.Exercise).Include(e => e.Exercise.Challenge)
                         .Include(e => e.Language)
                         .Where(e => e.ExerciseId == ExerciseId && e.Language.LanguageName.Contains(nameLangugae))
                         .ToList();
                    else if (ExerciseId != 0 && nameLangugae == null)
                        ls = _context.LanguageSupport.AsNoTracking()
                         .Include(e => e.Exercise).Include(e => e.Exercise.Challenge)
                         .Include(e => e.Language)
                         .Where(e => e.ExerciseId == ExerciseId)
                         .ToList();
                    else
                        ls = null;
                    if (ls == null)
                    {
                        return PartialView("ListLanguageSupSearchPartial", null);
                    }
                    else
                    {
                        return PartialView("ListLanguageSupSearchPartial", ls);
                    }
                }
            }
            return RedirectToAction("Login", "Accounts");

        }

    }
}
