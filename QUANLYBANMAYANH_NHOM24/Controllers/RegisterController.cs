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
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                return Json(new { success = false, errors });
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

                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
    }
}
