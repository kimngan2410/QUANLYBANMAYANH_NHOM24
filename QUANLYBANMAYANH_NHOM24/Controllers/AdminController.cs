using Microsoft.AspNetCore.Mvc;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult QuanLyTaiKhoan()
        {
            return View();
        }

        public IActionResult QuanLySanPham()
        {
            return View();  
        }
    }
}
