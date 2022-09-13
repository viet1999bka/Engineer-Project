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
    public class HomeController : Controller
    {
        private readonly EndProjectContext _context;

        public HomeController(EndProjectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var roleID = HttpContext.Session.GetString("RoleId");
            if (adminId != null)
            {
                var admin = _context.Account.AsNoTracking().Include(e => e.Role).SingleOrDefault(a => a.AccountId == Convert.ToInt32(adminId) && a.RoleId == Convert.ToInt32(roleID) && a.Lock == false);
                if (admin != null)
                {
                    var numUser = _context.Account.AsNoTracking().Where(x => x.RoleId == 3 && x.Lock == false).Count();
                    var listChallenge = _context.Challenge.AsNoTracking().ToList();
                    var numChallenge = listChallenge.Count();
                    var numExercise = _context.Exercise.AsNoTracking().Count();

                    Dictionary<string, int> dictDifficult = new Dictionary<string, int>();
                    var listDifficult = _context.Difficult.AsNoTracking().ToList();
                    foreach (var item in listDifficult)
                    {
                        var numEx = _context.Exercise.AsNoTracking().Where(e => e.LevelId == item.DifficultId).Count();
                        dictDifficult.Add(item.DifficultName.Trim(), numEx);
                    }

                    Dictionary<string, int> dictChalenge = new Dictionary<string, int>();

                    foreach (var item in listChallenge)
                    {
                        var numEx = _context.Exercise.AsNoTracking().Where(e => e.ChallengeId == item.ChallengeId).Count();
                        dictChalenge.Add(item.ChallengeName.Trim(), numEx);
                    }

                    var ordered = dictChalenge.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    dictChalenge.Clear();
                    int i = 0;
                    foreach (var item in ordered)
                    {
                        dictChalenge.Add(item.Key, item.Value);
                        i++;
                        if (i > 4) break;

                    }



                    ViewData["numuser"] = numUser;
                    ViewData["numChallenge"] = numChallenge;
                    ViewData["numExercise"] = numExercise;
                    ViewData["dictChalenge"] = dictChalenge;
                    ViewData["dictDifficult"] = dictDifficult;

                    return View(admin);
                }
            }
            return RedirectToAction("Login", "Accounts");
        }
    }
}
