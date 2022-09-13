using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Helpper;
using DATN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        private readonly EndProjectContext _context;
        public INotyfService _notyfService { get; }
        public AppController(EndProjectContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("chalenge.html", Name = "DanhSachCourse")]
        // GET: Danh sách các challenge
        public async Task<IActionResult> Challenge()
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var lsChallenge = await _context.Challenge.ToListAsync();
                    foreach (var item in lsChallenge)
                    {
                        var count = _context.Exercise
                                            .Where(o => o.ChallengeId == item.ChallengeId)
                                            .Count();
                        ViewData["totalchallenge" + item.ChallengeName] = count;
                    }
                    return View(await _context.Challenge.ToListAsync());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Thay đổi code dùng ajax

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCodeSubmit(int? exerciseID, string? language)
        {

            // Nếu đây là hàm thay đổi ngôn ngữ hỗ trợ 
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    if (language != null && exerciseID != null)
                    {
                        var url = $"CodeExercise.html?exerciseId={exerciseID}";
                        String CodeSub = "";

                        // Kiểm tra xem đã người dùng đã submit code ngôn ngữ này chưa
                        var getCodeMode = _context.ExerciseAttemp.AsNoTracking().Include(x => x.Language)
                            .Where(x => x.UserId == Convert.ToInt32(taiKhoanID) && x.ExerciseId == exerciseID && x.Language.LanguageMode == language)
                                                                        .FirstOrDefault();
                        if (getCodeMode != null)
                        {
                            // Nếu đã submit lấy code và trả về 
                            CodeSub = getCodeMode.CodeSubmit;
                            // Lấy thông tin phiên bản của ngôn ngữ
                            var versionLanguage = getCodeMode.Language.Version.Trim();
                            // Lấy thư viện sử dụng
                            var LanguageSupport = _context.LanguageSupport.AsNoTracking().SingleOrDefault(l => l.ExerciseId == exerciseID && l.Language.LanguageMode == language);
                            var libraryLanguage = LanguageSupport.Description.Trim();
                            var nameLanguage = getCodeMode.Language.LạnguageDisplay.Trim();
                            string[] listLibrary;
                            listLibrary = null;
                            if (libraryLanguage != null) listLibrary = libraryLanguage.Split("\\n");
                            string html = "<code>";
                            if (listLibrary != null)
                            {
                                foreach (var lib in listLibrary)
                                {
                                    html += lib;
                                }
                            }
                            html += "</code>";
                            url = "";
                            return Json(new { status = "success", codeSub = CodeSub, modeEdit = language, version = versionLanguage, html = listLibrary, name = nameLanguage });
                        }
                        else
                        {
                            // Nếu không
                            // Kiểm tra xem mode code này được hỗ trợ không
                            var checkMode = _context.LanguageSupport.AsNoTracking().Include(e => e.Exercise).Include(l => l.Language).Include(c => c.Exercise.Challenge)
                                                          .Where(x => x.ExerciseId == exerciseID && x.Language.LanguageMode == language)
                                                          .FirstOrDefault();
                            if (checkMode != null)
                            {
                                // Code được hỗ trợ thì đọc file và gửi code về 
                                string fileFunction = checkMode.FileFunction.Trim();
                                string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", checkMode.Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + checkMode.Exercise.ExcerciseName.Trim().ToLower());
                                string filePath = pathFolder + "\\" + checkMode.FileFunction.Trim();

                                try
                                {
                                    CodeSub = System.IO.File.ReadAllText(filePath);
                                }
                                catch
                                {
                                    _notyfService.Error("Bài tập này hiện đang bị lỗi. Xin thử lại sau");
                                    url = $"CodeExercise.html?exerciseId={exerciseID}";
                                    return Json(new { status = "error", returnUrl = url });
                                }
                                var versionLanguage = checkMode.Language.Version.Trim();
                                var libraryLanguage = checkMode.Description.Trim();
                                string[] listLibrary = null;
                                if (libraryLanguage != null) listLibrary = libraryLanguage.Split("\\n");
                                string html = "<code>";
                                if (listLibrary != null)
                                {
                                    foreach (var lib in listLibrary)
                                    {
                                        html += lib;
                                    }
                                }
                                html += "</code>";
                                var nameLanguage = checkMode.Language.LạnguageDisplay.Trim();

                                return Json(new { status = "success", codeSub = CodeSub, modeEdit = language, version = versionLanguage, html = listLibrary, name = nameLanguage });
                            }
                            else
                            {
                                _notyfService.Error("Ngôn ngữ này chưa được hỗ trợ");
                                url = $"CodeExercise.html?exerciseId={exerciseID}";
                                return Json(new { status = "error", returnUrl = url });
                            }
                        }

                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Exercise.html", Name = "DanhSachBaiTap")]
        public async Task<IActionResult> ListExercise(int? challengeId)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    if (challengeId == null)
                    {
                        return NotFound();
                    }

                    var lsExercise = await _context.Exercise.AsNoTracking().Include(e => e.Level)
                        .Where(x => x.ChallengeId == challengeId).ToListAsync();
                    var lsExAns = _context.ExerciseAttemp.AsNoTracking().Where(u => u.UserId == Convert.ToInt32(taiKhoanID) && u.Status == 1).Select(m => m.ExerciseId).Distinct().ToList();
                    ViewData["lsExAns"] = lsExAns;
                    if (lsExercise == null)
                    {
                        return NotFound();
                    }

                    return View(lsExercise);

                }
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("CodeExercise.html", Name = "TrangLamBaiTap")]
        public async Task<IActionResult> CodePage(int? exerciseId)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    if (exerciseId == null)
                    {
                        return NotFound();
                    }
                    // Kiểm tra xem bài tập tồn tại không
                    var inExercise = await _context.Exercise.AsNoTracking().Include(e => e.Challenge).SingleOrDefaultAsync(e => e.ExcerciseId == exerciseId);
                    if (inExercise == null)
                    {
                        _notyfService.Error("Không có bài tập này");
                        return RedirectToAction("Dashboard", "Accounts");
                    }

                    // Kiểm tra xem bài tập có những file code hỗ trợ ngôn ngữ nào
                    var lslanguage = await _context.LanguageSupport.AsNoTracking().Include(e => e.Language)
                        .Include(e => e.Exercise).Include(e => e.Exercise.Challenge)
                        .Where(e => e.ExerciseId == exerciseId).ToListAsync();

                    if (lslanguage.Count == 0)
                    {
                        _notyfService.Error("Bài tập này chưa có dữ liệu");
                        return RedirectToAction("Dashboard", "Accounts");
                    }

                    // Lấy ra các mode ngôn ngữ mà bài tập hỗ trợ truyền về client
                    Dictionary<string, string> modeData = new Dictionary<string, string>();
                    foreach (var item in lslanguage)
                    {
                        modeData.Add(item.Language.LanguageName.Trim(), item.Language.LanguageMode.Trim());
                    }
                    ViewData["modeData"] = modeData;

                    // Kiểm tra danh sách các test của bài tập
                    var lsTestCase = await _context.TestCase.AsNoTracking().Where(t => t.ExerciseId == exerciseId).OrderBy(x => x.Status).ToListAsync();
                    if (lsTestCase.Count == 0)
                    {
                        _notyfService.Error("Bài tập này hiện đang bị lỗi. Xin thử lại sau");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    ViewData["testCase"] = lsTestCase;


                    // Kiểm tra xem bài này đã được người dùng giải chưa
                    var oldExer = await _context.ExerciseAttemp.Include(l => l.Language).Where(x => x.UserId == Convert.ToInt32(taiKhoanID) && x.ExerciseId == exerciseId).FirstOrDefaultAsync();

                    // Nếu đã giải rồi thì hiển thị code đã submit và trả về ngôn ngữ người dùng đã giải bài đó để set mode 

                    if (oldExer != null)
                    {
                        ViewData["CodeSub"] = oldExer.CodeSubmit;
                        ViewData["DefaultMode"] = oldExer.Language.LanguageMode.Trim();
                        // Lấy thông tin phiên bản của ngôn ngữ
                        ViewData["versionLanguage"] = oldExer.Language.Version.Trim();
                        // Lấy thư viện sử dụng
                        var LanguageSupport = _context.LanguageSupport.AsNoTracking().SingleOrDefault(l => l.ExerciseId == exerciseId && l.LanguageId == oldExer.LanguageId);

                        ViewData["libraryLanguage"] = LanguageSupport.Description.Trim();
                        ViewData["nameLanguage"] = oldExer.Language.LạnguageDisplay.Trim();
                    }
                    // Chưa giải thì load file hàm lên
                    else
                    {
                        string fileFunction = lslanguage[0].FileFunction.Trim();
                        string pathfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", lslanguage[0].Exercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + lslanguage[0].Exercise.ExcerciseName.Trim().ToLower());
                        string filePath = pathfolder + "\\" + fileFunction;
                        try
                        {
                            ViewData["CodeSub"] = System.IO.File.ReadAllText(filePath);
                        }
                        catch
                        {
                            _notyfService.Error("Bài tập này hiện đang bị lỗi. Xin thử lại sau");
                            return RedirectToAction("Dashboard", "Accounts");
                        }
                        ViewData["DefaultMode"] = lslanguage[0].Language.LanguageMode.Trim();
                        // Lấy thông tin phiên bản của ngôn ngữ
                        ViewData["versionLanguage"] = lslanguage[0].Language.Version.Trim();
                        // Lấy thư viện sử dụng
                        ViewData["libraryLanguage"] = lslanguage[0].Description.Trim();
                        // Lấy tên ngôn ngữ
                        ViewData["nameLanguage"] = lslanguage[0].Language.LạnguageDisplay.Trim();

                    }

                    return View(inExercise);


                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CodeExercise.html", Name = "TrangLamBaiTap")]
        public async Task<IActionResult> CodePage(int exerciseId, string codeSubmit, string languageMode, string inputSubmit)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    if (exerciseId == null)
                    {
                        return NotFound();
                    }
                    // Kiểm tra bài tập tồn tại không
                    var inExercise = await _context.Exercise.AsNoTracking().Include(e => e.Challenge).SingleOrDefaultAsync(e => e.ExcerciseId == exerciseId);
                    if (inExercise == null)
                    {
                        return RedirectToAction("CodePage", new { exerciseId = exerciseId });
                    }

                    // Đường dẫn thư mục chứa code

                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "codeFile", inExercise.Challenge.ChallengeName.Trim().ToLower() + "\\" + inExercise.ExcerciseName.Trim().ToLower());

                    // Lấy ra danh sách test case thuộc bài tập
                    var lsTestCase = await _context.TestCase.AsNoTracking().Where(t => t.ExerciseId == exerciseId).OrderBy(x => x.Status).ToListAsync();
                    if (lsTestCase.Count == 0)
                    {
                        _notyfService.Error("Bài tập này hiện đang bị lỗi. Xin thử lại sau");
                        return RedirectToAction("Dashboard", "Accounts");
                    }

                    // Đưa test case vào dạng list <input, output>
                    var tupleListCase = new List<(string, string)>();
                    foreach (var item in lsTestCase)
                    {
                        tupleListCase.Add((item.Input, item.Output));
                    }

                    ViewData["testCase"] = lsTestCase;
                    // Hiển thị code cũ
                    ViewData["CodeSub"] = codeSubmit;
                    // Set default mode = mode vừa gửi 
                    ViewData["DefaultMode"] = languageMode.Trim();



                    // Kiểm tra xem bài tập có những file code hỗ trợ ngôn ngữ nào
                    var lslanguage = await _context.LanguageSupport.AsNoTracking().Include(e => e.Language)
                        .Include(e => e.Exercise).Include(e => e.Exercise.Challenge)
                        .Where(e => e.ExerciseId == exerciseId).ToListAsync();
                    if (lslanguage.Count == 0)
                    {
                        _notyfService.Error("Bài tập này chưa có dữ liệu");
                        return RedirectToAction("Dashboard", "Accounts");
                    }

                    // Lấy ra các mode ngôn ngữ mà bài tập hỗ trợ truyền về client
                    Dictionary<string, string> modeData = new Dictionary<string, string>();
                    foreach (var item in lslanguage)
                    {
                        modeData.Add(item.Language.LanguageName.Trim(), item.Language.LanguageMode.Trim());
                    }
                    ViewData["modeData"] = modeData;

                    // Kiểm tra xem ngôn ngữ khi post có trong danh sách ngôn ngữ được hỗ trợ không

                    if (!modeData.ContainsValue(languageMode))
                    {
                        _notyfService.Error("Ngôn ngữ này không được hỗ trợ");
                        return RedirectToAction("CodePage", new { exerciseId = exerciseId });
                    }

                    // lấy ra phần tử chứa mode của ngôn ngữ gửi lên để lấy phần đuôi mở rộng
                    var languageItem = lslanguage.Find(x => x.Language.LanguageMode.Trim() == languageMode);
                    // Lấy thông tin phiên bản của ngôn ngữ
                    ViewData["versionLanguage"] = languageItem.Language.Version.Trim();
                    // Lấy thư viện sử dụng
                    ViewData["libraryLanguage"] = languageItem.Description.Trim();
                    // Lấy tên ngôn ngữ sử dụng
                    ViewData["nameLanguage"] = languageItem.Language.LạnguageDisplay.Trim();

                    var languageExten = languageItem.Language.LanguageExten.Trim();
                    // Biển lưu kết quả output từng test case
                    var resTestCase = new List<Tuple<string, string>>();
                    // Kiểm tra xem là phương thức submit hay runtest



                    if (inputSubmit == "submit")
                    {
                        // Phương thức submit nên lấy ra hết test case gồm cả ẩn và hiện
                        var numTest = lsTestCase.LongCount();
                        var res = Utlities.getResults(location, languageItem.FileMain.Trim(), tupleListCase, codeSubmit, taiKhoanID, languageExten, inExercise.TimeLimit.Trim());
                        ViewData["res"] = res;
                        if (res == null)
                        {
                            ViewData["numRight"] = "Failed all test";
                            return View(inExercise);
                        }
                        //res.Trim();
                        //var listRest = res.Split(' ');
                        //var numTrue = listRest.Where(x => x == "true").Count();

                        if (res.Count < tupleListCase.Count)
                        {
                            ViewData["numRight"] = "Failed num res < num test case";
                            return View(inExercise);
                        }

                        // Kiểm tra kết quả trả về với từng test case 
                        var numTrue = 0;
                        // Biến lưu giá trị kết quả của lời giải với từng test case

                        for (int i = 0; i < tupleListCase.Count; i++)
                        {
                            if (res[i].TrimStart().TrimEnd() == tupleListCase[i].Item2)
                            {
                                numTrue++;
                                resTestCase.Add(Tuple.Create(res[i].TrimStart().TrimEnd(), "true"));

                            }
                            else
                                resTestCase.Add(Tuple.Create(res[i].TrimStart().TrimEnd(), "false"));
                        }
                        ViewData["resTestCase"] = resTestCase;


                        if (numTrue == 0)
                        {
                            ViewData["numRight"] = "Failed all test";
                            return View(inExercise);
                        }

                        ViewData["numRight"] = numTrue.ToString() + "/" + numTest.ToString();

                        if (numTrue == numTest)
                        {
                            // Kiểm tra xem đã submit code của ngôn ngữ này trước đó chưa
                            var oldExer = await _context.ExerciseAttemp.Where(x => x.UserId == Convert.ToInt32(taiKhoanID) && x.ExerciseId == exerciseId && x.Language.LanguageMode == languageMode).FirstOrDefaultAsync();
                            if (oldExer != null)
                            {
                                // Nếu đã submit thì cập nhật lại
                                oldExer.CodeSubmit = codeSubmit;
                                oldExer.TimeAttemp = DateTime.Now;
                                _context.ExerciseAttemp.Update(oldExer);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                // Nếu là lần đầu submit thì thêm vào database
                                // Lấy ra thông tin về ngôn ngữ để thêm vào database
                                var languagInfor = _context.Language.AsNoTracking().SingleOrDefault(x => x.LanguageMode == languageMode.Trim());
                                if (languagInfor == null)
                                {
                                    _notyfService.Error("Bài tập bị lỗi");
                                    return View(inExercise);
                                }
                                ExerciseAttemp exerciseAttemp = new ExerciseAttemp();
                                exerciseAttemp.UserId = Convert.ToInt32(taiKhoanID);
                                exerciseAttemp.ExerciseId = exerciseId;
                                exerciseAttemp.LanguageId = languagInfor.LanguageId;
                                exerciseAttemp.CodeSubmit = codeSubmit;
                                exerciseAttemp.TimeAttemp = DateTime.Now;
                                exerciseAttemp.Status = 1;
                                _context.ExerciseAttemp.Add(exerciseAttemp);
                                await _context.SaveChangesAsync();
                            }


                        }

                    }
                    // Nếu là run test
                    else
                    {

                        var numTest = 0;
                        // Đưa test case vào dạng list <input, output>
                        tupleListCase = new List<(string, string)>();
                        foreach (var item in lsTestCase)
                        {
                            if (item.Status == false)
                            {
                                tupleListCase.Add((item.Input.Trim(), item.Output.Trim()));
                                numTest++;
                            }
                        }
                        var res = Utlities.getResults(location, languageItem.FileMain, tupleListCase, codeSubmit, taiKhoanID, languageExten, inExercise.TimeLimit.Trim());
                        if (res == null)
                        {
                            ViewData["numRight"] = "Failed all test";
                            return View(inExercise);
                        }

                        //res.Trim();
                        //var listRest = res.Split(' ');
                        //var numTrue = listRest.Where(x => x == "true").Count();

                        if (res.Count < tupleListCase.Count)
                        {
                            ViewData["numRight"] = "Failed num res < num test case";
                            return View(inExercise);
                        }
                        // Kiểm tra kết quả trả về với từng test case 
                        var numTrue = 0;
                        for (int i = 0; i < tupleListCase.Count; i++)
                        {

                            if (res[i].Trim() == tupleListCase[i].Item2.Trim())
                            {
                                numTrue++;
                                resTestCase.Add(Tuple.Create(res[i].TrimStart().TrimEnd(), "true"));

                            }
                            else
                            {
                                resTestCase.Add(Tuple.Create(res[i].TrimStart().TrimEnd(), "false"));
                            }
                        }

                        ViewData["resTestCase"] = resTestCase;

                        ViewData["numRight"] = numTrue.ToString() + "/" + numTest.ToString();
                    }

                    return View(inExercise);


                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
