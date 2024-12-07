using Microsoft.AspNetCore.Mvc;
using QUANLYBANMAYANH_NHOM24.Models;
using System.Linq;

namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class KhachVangLaiController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public KhachVangLaiController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? idDanhMucConCanon, int? idDanhMucConSony)
        {
            // Lấy sản phẩm Canon
            var canonSanPhams = _context.SanPhams
                                        .Where(sp => sp.Idhang == 1 &&
                                                     (!idDanhMucConCanon.HasValue || sp.Iddanhmuccon == idDanhMucConCanon))
                                        .ToList();

            // Lấy danh mục con Canon
            var canonDanhMucCon = _context.DanhMucCons
                                          .Where(dm => _context.SanPhams.Any(sp => sp.Iddanhmuccon == dm.Iddanhmuccon && sp.Idhang == 1))
                                          .ToList();

            // Lấy sản phẩm Sony
            var sonySanPhams = _context.SanPhams
                                       .Where(sp => sp.Idhang == 2 &&
                                                    (!idDanhMucConSony.HasValue || sp.Iddanhmuccon == idDanhMucConSony))
                                       .ToList();

            // Lấy danh mục con Sony
            var sonyDanhMucCon = _context.DanhMucCons
                                         .Where(dm => _context.SanPhams.Any(sp => sp.Iddanhmuccon == dm.Iddanhmuccon && sp.Idhang == 2))
                                         .ToList();

            // Gửi dữ liệu qua ViewBag
            ViewBag.CanonSanPhams = canonSanPhams;
            ViewBag.CanonDanhMucCon = canonDanhMucCon;

            ViewBag.SonySanPhams = sonySanPhams;
            ViewBag.SonyDanhMucCon = sonyDanhMucCon;

            return View();
        }
    }
}
