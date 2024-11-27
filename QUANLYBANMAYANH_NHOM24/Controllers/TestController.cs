using Microsoft.AspNetCore.Mvc;
using QUANLYBANMAYANH_NHOM24.Models;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class TestController: Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public TestController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var danhMuc = _context.DanhMucs.ToList(); // Lấy tất cả các danh mục
            return View(danhMuc);
        }
    }
}
