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

        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel model)
        {
            // Kiểm tra nếu model không hợp lệ
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();

                return Json(new
                {
                    success = false,
                    message = string.Join(" ", errorMessages) // Gộp các lỗi lại
                });
            }

            // Kiểm tra thông tin người dùng trong cơ sở dữ liệu
            var user = _context.NguoiDungs
                               .FirstOrDefault(u => u.Email == model.Email && u.Matkhau == model.MatKhau);

            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Email hoặc mật khẩu không đúng."
                });
            }

            // Đăng nhập thành công: lưu thông tin người dùng vào Session
            HttpContext.Session.SetString("UserEmail", user.Email);

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "KhachVangLai") // URL điều hướng sau khi đăng nhập thành công
            });
        }
    }
}
