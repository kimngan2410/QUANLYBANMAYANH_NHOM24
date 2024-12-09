using Microsoft.AspNetCore.Mvc;
using QUANLYBANMAYANH_NHOM24.Models;
using System.Linq;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class RegisterController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public RegisterController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Trả về PartialView với form đăng ký
            return PartialView("_LoginPartial", new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        field = ms.Key, // Tên trường gây lỗi
                        message = ms.Value.Errors.FirstOrDefault()?.ErrorMessage // Thông báo lỗi
                    })
                    .ToList();

                return Json(new { success = false, errors });
            }
            // Kiểm tra email đã tồn tại
            var existingUser = _context.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return Json(new
                {
                    success = false,
                    errors = new List<object>
                {
                new { field = "Email", message = "Email đã tồn tại trong hệ thống." }
                    }
                });
            }
            try
            {
                var newUser = new NguoiDung
                {
                    TenNguoiDung = model.TenNguoiDung,
                    Email = model.Email,
                    Sdt = model.Sdt,
                    Matkhau = model.MatKhau,
                    Gioitinh = model.GioiTinh,
                    Ngaysinh = DateOnly.FromDateTime(model.NgaySinh)
                };

                _context.NguoiDungs.Add(newUser);
                _context.SaveChanges();
                Console.WriteLine("Thêm thành công người dùng mới!");
                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                // Log lỗi ra console hoặc file
                Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
    }
}
