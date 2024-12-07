using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QUANLYBANMAYANH_NHOM24.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QUANLYBANMAYANH_NHOM24.Components
{
    public class DanhMucViewComponent : ViewComponent
    {
        private readonly QuanLyBanMayAnhContext _context;

        public DanhMucViewComponent(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh mục và danh mục con từ cơ sở dữ liệu
            var danhMucs = await _context.DanhMucs
                .Include(d => d.DanhMucCons) // Bao gồm danh mục con
                .ToListAsync();

            // Trả về view của ViewComponent với dữ liệu danh mục
            return View(danhMucs);
        }
    }
}