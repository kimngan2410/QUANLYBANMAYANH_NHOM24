using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Để sử dụng HttpContext.Session
using QUANLYBANMAYANH_NHOM24.Models;
using System.Linq;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class LoginController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public LoginController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Trả về PartialView với form đăng nhập
            return PartialView("_LoginPartial", new LoginViewModel());
        }

        [HttpGet]
        public IActionResult GetLoginStatus()
        {
            var userName = HttpContext.Session.GetString("LoggedInUser");
            var avatarUrl = HttpContext.Session.GetString("UserAvatar") ?? Url.Content("~/images/default-avatar.png");

            // Debug thông tin Session
            Console.WriteLine($"HIHI - LoggedInUser: {userName}");
            Console.WriteLine($"HAHA - UserAvatar: {avatarUrl}");

            return Json(new
            {
                isLoggedIn = !string.IsNullOrEmpty(userName),
                userName = userName ?? "Guest", // Giá trị mặc định nếu chưa đăng nhập
                avatarUrl = avatarUrl
            });
        }


        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                return Json(new { success = false, message = string.Join(" ", errorMessages) });
            }

            var user = _context.NguoiDungs
                               .FirstOrDefault(u => u.Email == model.Email && u.Matkhau == model.MatKhau);

            if (user == null)
            {
                return Json(new { success = false, message = "Email hoặc mật khẩu không đúng." });
            }

            // Lưu IdNguoiDung vào session
            HttpContext.Session.SetInt32("IdNguoiDung", user.Idnguoidung); // Lưu ID người dùng
            HttpContext.Session.SetString("LoggedInUser", user.TenNguoiDung); // Lưu tên người dùng
            HttpContext.Session.SetString("UserAvatar", user.AnhNguoiDung ?? Url.Content("~/images/default-avatar.png"));// Lưu ảnh đại diện hoặc ảnh mặc định

            Console.WriteLine($"LoggedInUser: {HttpContext.Session.GetString("LoggedInUser")}");
            Console.WriteLine($"UserAvatar: {HttpContext.Session.GetString("UserAvatar")}");


            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "KhachVangLai"),
                avatarUrl = HttpContext.Session.GetString("UserAvatar"),
                userName = HttpContext.Session.GetString("LoggedInUser")
            });
        }

        public IActionResult DangXuat()
        {
            // Xóa toàn bộ thông tin trong session
            HttpContext.Session.Clear();

            // Điều hướng trở lại giao diện khách vãng lai
            return RedirectToAction("Index", "KhachVangLai");
        }
        public IActionResult Index()
        {
            Console.WriteLine("Index method called");
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            var userAvatar = HttpContext.Session.GetString("UserAvatar") ?? Url.Content("~/images/default-avatar.png");

            ViewBag.LoggedInUser = loggedInUser;
            ViewBag.UserAvatar = userAvatar;

            // Trả về trạng thái 204 - No Content
            return View();
        }
    }
}
