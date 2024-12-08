using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QUANLYBANMAYANH_NHOM24.Models;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class TestController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public TestController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách danh mục và danh mục con
            var danhMucs = _context.DanhMucs
                .Include(dm => dm.DanhMucCons) // Gộp danh sách con
                .ToList();
            
            return View(danhMucs);
        }
    }
}