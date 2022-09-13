using AspNetCoreHero.ToastNotification.Abstractions;
using DATN.Extension;
using DATN.Helpper;
using DATN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Controllers
{
    [Authorize]

    public class UserProfileController : Controller
    {
        private readonly EndProjectContext _context;
        private readonly INotyfService _notyf;
        public UserProfileController(EndProjectContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var lsExperience = await _context.Experience.AsNoTracking().Where(t => t.UserId == khachhang.AccountId).OrderBy(x => x.ExperienceId).ToListAsync();
                    var lsEducation = await _context.Education.AsNoTracking().Where(t => t.UserId == khachhang.AccountId).OrderBy(x => x.EducationId).ToListAsync();
                    var lsExercise = await _context.ExerciseAttemp.AsNoTracking().Include(e => e.Exercise).Include(l => l.Language).Where(t => t.UserId == khachhang.AccountId).OrderBy(x => x.TimeAttemp).Take(5).ToListAsync();
                    var lsCodeFriend = await _context.CodeFriend.AsNoTracking().Include(e => e.Friend).Where(t => t.UserId == khachhang.AccountId && t.Status == 1).Take(5).ToListAsync();
                    var numberMyFriendRequest = _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == khachhang.AccountId && u.Status == 0).Count();

                    ViewData["lsExperience"] = lsExperience;
                    ViewData["lsEducation"] = lsEducation;
                    ViewData["lsExercise"] = lsExercise;
                    ViewData["lsCodeFriend"] = lsCodeFriend;
                    ViewData["numberOfRequest"] = numberMyFriendRequest;

                    return View(khachhang);
                }
                _notyf.Error("Lỗi đăng nhập");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> AddExperience(string company, string jobTitle, string jobLocation, string startDate, string startYear, string endDate, string endYear)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                if (ModelState.IsValid)
                {
                    Experience experience = new Experience();
                    experience.UserId = Convert.ToInt32(taiKhoanID);
                    string strStartTime = startDate + "/" + startYear;
                    string strEndTime = endDate + "/" + endYear;
                    DateTime startTime = DateTime.ParseExact(strStartTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime endTime = DateTime.ParseExact(strEndTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    experience.StartDate = startTime;
                    experience.EndDate = endTime;
                    experience.Company = company;
                    experience.JobTitle = jobTitle;
                    experience.JobLocation = jobLocation;
                    _context.Add(experience);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Thêm trải nghiệm thành công");
                    return RedirectToAction(nameof(Index));
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> AddEducation(string university, string degree, string subject, string startDate, string startYear, string endDate, string endYear)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                if (ModelState.IsValid)
                {
                    Education education = new Education();
                    education.UserId = Convert.ToInt32(taiKhoanID);

                    education.University = university;
                    education.Degree = degree;
                    education.Subject = subject;
                    _context.Add(education);
                    string strStartTime = startDate + "/" + startYear;
                    string strEndTime = endDate + "/" + endYear;
                    DateTime startTime = DateTime.ParseExact(strStartTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime endTime = DateTime.ParseExact(strEndTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    education.StartDate = startTime;
                    education.EndDate = endTime;
                    await _context.SaveChangesAsync();
                    _notyf.Success("Thêm học vấn thành công");
                    return RedirectToAction(nameof(Index));
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteExperience")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> DeleteExperience(int experienceId = -1)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                if (experienceId >= 0)
                {
                    var experience = await _context.Experience.FindAsync(experienceId);
                    if (experience != null)
                    {
                        _context.Experience.Remove(experience);
                        await _context.SaveChangesAsync();
                        _notyf.Success("Xóa thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("DeleteEducation")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> DeleteEducation(int educationId = -1)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                if (educationId >= 0)
                {
                    var education = await _context.Education.FindAsync(educationId);
                    if (education != null)
                    {
                        _context.Education.Remove(education);
                        await _context.SaveChangesAsync();
                        _notyf.Success("Xóa thành công");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                        return RedirectToAction(nameof(Index));
                    }
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("EditExperience")]
        [AllowAnonymous]

        public async Task<IActionResult> EditExperience(int id, string company, string jobTitle, string jobLocation, string startDate, string startYear, string endDate, string endYear)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var experience = await _context.Experience.FindAsync(id);
                if (experience != null)
                {
                    experience.Company = company;
                    experience.JobTitle = jobTitle;
                    experience.JobLocation = jobLocation;
                    string strStartTime = startDate + "/" + startYear;
                    string strEndTime = endDate + "/" + endYear;
                    DateTime startTime = DateTime.ParseExact(strStartTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime endTime = DateTime.ParseExact(strEndTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    experience.StartDate = startTime;
                    experience.EndDate = endTime;
                    _context.Experience.Update(experience);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Chỉnh sửa thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notyf.Error("Có lỗi xảy ra");
                    return RedirectToAction(nameof(Index));
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("EditEducation")]
        [AllowAnonymous]
        public async Task<IActionResult> EditEduction(int id, string university, string degree, string subject, string startDate, string startYear, string endDate, string endYear)
        {
            // Kiểm tra xem đã đăng nhập chưa
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var education = await _context.Education.FindAsync(id);
                if (education != null)
                {
                    education.University = university;
                    education.Degree = degree;
                    education.Subject = subject;
                    string strStartTime = startDate + "/" + startYear;
                    string strEndTime = endDate + "/" + endYear;
                    DateTime startTime = DateTime.ParseExact(strStartTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime endTime = DateTime.ParseExact(strEndTime, "MM/yyyy", CultureInfo.InvariantCulture);
                    education.StartDate = startTime;
                    education.EndDate = endTime;
                    _context.Education.Update(education);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Chỉnh sửa thành công");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notyf.Error("Có lỗi xảy ra");
                    return RedirectToAction(nameof(Index));
                }
                _notyf.Error("Có lỗi xảy ra");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("EditUserInfor")]
        [AllowAnonymous]
        public async Task<IActionResult> EditUserInfor(string fullname, string phone, DateTime? birthday, string address)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {

                var khachhang = _context.Account.AsNoTracking().FirstOrDefault(x => x.Phone.ToLower() == phone);
                if (khachhang != null && khachhang.AccountId != Convert.ToInt32(taiKhoanID))
                {
                    TempData["ErrorPhone"] = "Số điện thoại đã được sử dụng";
                    return RedirectToAction(nameof(Index));
                }

                if (!Utlities.IsDigitsOnly(phone) || phone.Length > 15 || phone.Length < 9)
                {
                    TempData["ErrorPhone"] = "Số điện thoại không hợp lệ";
                    return RedirectToAction(nameof(Index));
                }


                var user = await _context.Account.FindAsync(Convert.ToInt32(taiKhoanID));
                user.FullName = fullname;
                user.Phone = phone;
                user.Address = address;

                if (birthday != null)
                {
                    user.BirthDay = birthday.Value.Date;
                }
                _context.Account.Update(user);
                await _context.SaveChangesAsync();
                _notyf.Success("Chỉnh sửa thành công");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("ChangePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string oldpass, string newpass)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {

                    var pass = (oldpass + khachhang.Salt.Trim()).ToMD5();
                    if (khachhang.Password != pass)
                    {
                        TempData["errorOldPass"] = "Mật khẩu không đúng";
                        return RedirectToAction(nameof(Index));
                    }

                    if (newpass == oldpass)
                    {
                        TempData["errorNewPass"] = "Mật khẩu mới phải khác mật khẩu đang sử dụng";
                        return RedirectToAction(nameof(Index));
                    }

                    if (!Utlities.IsValidPass(newpass))
                    {
                        TempData["errorNewPass"] = "Mật khẩu phải dài tối thiểu 8 ký tự và chứa ít nhất một chữ hoa, một chữ thường, một ký tự đặc biệt và không chứa khoảng trắng";
                        return RedirectToAction(nameof(Index));
                    }

                    pass = (newpass + khachhang.Salt.Trim()).ToMD5();
                    khachhang.Password = pass;
                    _context.Account.Update(khachhang);
                    await _context.SaveChangesAsync();
                    _notyf.Success("Thay đổi mật khẩu thành công");
                    return RedirectToAction(nameof(Index));

                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("ChangeAvatar")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeAvatar(Microsoft.AspNetCore.Http.IFormFile fAvatar)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {

                    if (fAvatar != null)
                    {
                        string extension = Path.GetExtension(fAvatar.FileName);
                        string image = khachhang.Email.Trim().ToLower() + extension;
                        khachhang.Avatar = await Utlities.UploadFile(fAvatar, @"AccountAvatars", image);
                        _context.Account.Update(khachhang);
                        await _context.SaveChangesAsync();
                        _notyf.Success("Chỉnh sửa thành công");
                    }

                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("MyCodeFriend")]
        [AllowAnonymous]
        public async Task<IActionResult> MyCodeFriend()
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var listFriend = await _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == khachhang.AccountId && u.Status == 1).ToListAsync();
                    var numberMyFriendRequest = _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == khachhang.AccountId && u.Status == 0).Count();
                    ViewData["numberOfRequest"] = numberMyFriendRequest;

                    return View(listFriend);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, ActionName("MyFriendRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> MyFriendRequest()
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var listMyFriendRequest = await _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == khachhang.AccountId && u.Status == 0).ToListAsync();
                    var listMyFriendSend = await _context.CodeFriend.AsNoTracking().Include(f => f.Friend).Where(u => u.UserId == khachhang.AccountId && u.Status == 2).ToListAsync();

                    ViewData["listMyFriendRequest"] = listMyFriendRequest;
                    ViewData["listMyFriendSend"] = listMyFriendSend;
                    int friendId = 0;
                    if (TempData["Search"] != null)
                    {
                        ViewData["Search"] = "Có";
                        if (TempData["friend"] != null)
                        {
                            friendId = Convert.ToInt32(TempData["friend"].ToString());

                        }
                    }

                    var friend = _context.Account.AsNoTracking()
                        .SingleOrDefault(e => e.AccountId == friendId);
                    ViewData["friend"] = friend;
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("SendFriendRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> SendFriendRequest(int id, int view = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var checkCorrelation = _context.CodeFriend.AsNoTracking().SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id);
                    if (checkCorrelation != null)
                    {
                        _notyf.Error("Có lỗi xảy ra. Không thể gửi lời mời");
                    }
                    else
                    {
                        var userSend = new CodeFriend
                        {
                            UserId = khachhang.AccountId,
                            FriendId = id,
                            Status = 2,
                            DateJoin = DateTime.Now
                        };

                        var friendRev = new CodeFriend
                        {
                            UserId = id,
                            FriendId = khachhang.AccountId,
                            Status = 0,
                            DateJoin = DateTime.Now
                        };

                        _context.CodeFriend.Add(userSend);
                        _context.CodeFriend.Add(friendRev);
                        await _context.SaveChangesAsync();
                        _notyf.Success("Gửi lời mời kết bạn thành công");
                    }

                    if (view == 0)
                    {
                        return RedirectToAction("MyFriendRequest");
                    }
                    return RedirectToAction("Index", "Others", new { id = id });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, ActionName("AcceptFriendRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptFriendRequest(int id, int view = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var needAccept = _context.CodeFriend.AsNoTracking().Include(e => e.Friend).SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id);
                    if (needAccept != null)
                    {
                        if (needAccept.Status == 0)
                        {
                            var friendRequest = _context.CodeFriend.AsNoTracking().SingleOrDefault(u => u.UserId == id && u.FriendId == khachhang.AccountId && u.Status == 2);
                            if (friendRequest != null)
                            {
                                friendRequest.Status = 1;
                                _context.CodeFriend.Update(friendRequest);
                            }
                            needAccept.Status = 1;
                            _context.CodeFriend.Update(needAccept);
                            await _context.SaveChangesAsync();
                            _notyf.Success("Bạn và " + needAccept.Friend.FullName.Trim() + " đã trở thành bạn bè");
                        }
                        else
                        {
                            _notyf.Error("Có lỗi xảy ra");
                        }
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                    }

                    if (view == 0)
                    {
                        return RedirectToAction("MyFriendRequest");
                    }
                    return RedirectToAction("Index", "Others", new { id = id });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Từ chối kết bạn
        [HttpGet, ActionName("RefuseFriend")]
        [AllowAnonymous]
        public async Task<IActionResult> RefuseFriend(int id, int view = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var userRequest = _context.CodeFriend.AsNoTracking().Include(e => e.Friend).SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id && u.Status == 0);
                    if (userRequest != null)
                    {
                        var friendRequest = _context.CodeFriend.AsNoTracking().SingleOrDefault(u => u.UserId == id && u.FriendId == khachhang.AccountId && u.Status == 2);
                        _context.CodeFriend.Remove(userRequest);
                        if (friendRequest != null)
                        {
                            _context.CodeFriend.Remove(friendRequest);
                        }
                        await _context.SaveChangesAsync();
                        _notyf.Success("Thao tác thành công");
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                    }

                    if (view == 0)
                    {
                        return RedirectToAction("MyFriendRequest");
                    }
                    return RedirectToAction("Index", "Others", new { id = id });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Xóa kết bạn
        [HttpGet, ActionName("DeleteFriend")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteFriend(int id, int view = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var userRequest = _context.CodeFriend.AsNoTracking().Include(e => e.Friend).SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id && u.Status == 1);
                    if (userRequest != null)
                    {
                        var friendRequest = _context.CodeFriend.AsNoTracking().SingleOrDefault(u => u.UserId == id && u.FriendId == khachhang.AccountId && u.Status == 1);
                        _context.CodeFriend.Remove(userRequest);
                        if (friendRequest != null)
                        {
                            _context.CodeFriend.Remove(friendRequest);
                        }
                        await _context.SaveChangesAsync();
                        _notyf.Success("Thao tác thành công");
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                    }

                    if (view == 0)
                    {
                        return RedirectToAction("MyFriendRequest");
                    }
                    return RedirectToAction("Index", "Others", new { id = id });
                }
            }
            return RedirectToAction("Index", "Home");
        }


        // Hủy lời mời
        [HttpGet, ActionName("CancelFriend")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelFriend(int id, int view = 0)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    var userRequest = _context.CodeFriend.AsNoTracking().Include(e => e.Friend).SingleOrDefault(u => u.UserId == khachhang.AccountId && u.FriendId == id && u.Status == 2);
                    if (userRequest != null)
                    {
                        var friendRequest = _context.CodeFriend.AsNoTracking().SingleOrDefault(u => u.UserId == id && u.FriendId == khachhang.AccountId && u.Status == 0);
                        _context.CodeFriend.Remove(userRequest);
                        if (friendRequest != null)
                        {
                            _context.CodeFriend.Remove(friendRequest);
                        }
                        await _context.SaveChangesAsync();
                        _notyf.Success("Thao tác thành công");
                    }
                    else
                    {
                        _notyf.Error("Có lỗi xảy ra");
                    }

                    if (view == 0)
                    {
                        return RedirectToAction("MyFriendRequest");
                    }
                    return RedirectToAction("Index", "Others", new { id = id });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // Tìm kiếm người dùng
        [HttpPost, ActionName("SearchFriend")]
        [AllowAnonymous]
        public IActionResult SearchFriend(string email)
        {
            var taiKhoanID = HttpContext.Session.GetString("CustomerId");
            if (taiKhoanID != null)
            {
                var khachhang = _context.Account.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taiKhoanID));
                if (khachhang != null)
                {
                    TempData["Search"] = "Có";
                    if (string.IsNullOrEmpty(email) || email.Length < 1)
                    {
                        TempData["friend"] = 0;
                    }
                    var friend = _context.Account.AsNoTracking()
                        .SingleOrDefault(e => e.Email == email && e.RoleId == 3);
                    if (friend != null)
                    {
                        TempData["friend"] = friend.AccountId;
                    }
                    else
                    {
                        TempData["friend"] = 0;
                    }
                    return RedirectToAction("MyFriendRequest");

                }
            }
            return RedirectToAction("Index", "Home");


        }
    }
}
