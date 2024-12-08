using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QUANLYBANMAYANH_NHOM24.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using System.Security.Claims;


namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;

        public KhachHangController(QuanLyBanMayAnhContext context)
        {
            _context = context;
        }



        public IActionResult GioHang()
        {
            return View();
        }



        /*-----------------GIỎ HÀNG---------------*/
        /*- Thêm giỏ hàng -*/
        [HttpPost]
        public IActionResult ThemVaoGioHang(int idSanPham, int soLuong)
        {
            // Sử dụng giá trị mặc định cho Idnguoidung
            int defaultUserId = 1;

            var gioHang = _context.GioHangs.FirstOrDefault(gh => gh.Idnguoidung == defaultUserId && gh.Idsanpham == idSanPham);

            if (gioHang != null)
            {
                gioHang.Soluong += soLuong;
                _context.GioHangs.Update(gioHang);
            }
            else
            {
                var newGioHang = new GioHang
                {
                    Idnguoidung = defaultUserId,
                    Idsanpham = idSanPham,
                    Soluong = soLuong
                };
                _context.GioHangs.Add(newGioHang);
            }
            _context.SaveChanges();


            // Tính lại số lượng sản phẩm trong giỏ hàng
            int cartCount = _context.GioHangs.Where(gh => gh.Idnguoidung == defaultUserId).Sum(gh => gh.Soluong);

            return Json(new { success = true, cartCount = cartCount });
        }


        /*--- Cập nhật số lượng sản phẩm trong giỏ hàng ---*/
        [HttpGet]
        public IActionResult LaySoLuongGioHang()
        {
            int defaultUserId = 1; // ID người dùng mặc định
            int cartCount = _context.GioHangs.Where(gh => gh.Idnguoidung == defaultUserId).Sum(gh => gh.Soluong);

            return Json(new { success = true, cartCount = cartCount });
        }



        public IActionResult GioHang_2()
        {
            int userId = 1; // Thay bằng Idnguoidung thực tế của người dùng hiện tại

            var gioHang = _context.GioHangs
                .Where(gh => gh.Idnguoidung == userId)
                .Include(gh => gh.IdsanphamNavigation)
                .Select(gh => new GioHangViewModel
                {
                    IdSanPham = gh.IdsanphamNavigation.Idsanpham,
                    TenSanPham = gh.IdsanphamNavigation.Tensp,
                    Gia = gh.IdsanphamNavigation.Gia,
                    DiaChiAnh = gh.IdsanphamNavigation.DiachianhSp,
                    SoLuong = gh.Soluong,
                    ThanhTien = gh.Soluong * gh.IdsanphamNavigation.Gia
                })
                .ToList();

            // Tính tổng thành tiền của tất cả các sản phẩm trong giỏ hàng
            decimal tongThanhTien = gioHang.Sum(g => g.ThanhTien);

            // Tính tổng số lượng sản phẩm trong giỏ hàng
            int tongSoLuong = gioHang.Sum(g => g.SoLuong);

            if (!gioHang.Any())
            {
                return RedirectToAction("GioHang");
            }

            // Truyền dữ liệu xuống View, bao gồm giỏ hàng và tổng thành tiền
            ViewBag.TongThanhTien = tongThanhTien;
            ViewBag.TongSoLuong = tongSoLuong;

            return View(gioHang);
        }

        [HttpPost]
        public IActionResult CapNhatSoLuong([FromBody] CapNhatSoLuongRequest request)
        {
            try
            {
                // Kiểm tra request có phải là null không
                if (request == null)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                int defaultUserId = 1;

                var gioHang = _context.GioHangs
                    .Include(gh => gh.IdsanphamNavigation) // Bắt buộc phải nạp thông tin chi tiết sản phẩm
                    .FirstOrDefault(gh => gh.Idnguoidung == defaultUserId && gh.Idsanpham == request.IdSanPham);


                // Kiểm tra xem giỏ hàng có tồn tại không
                if (gioHang == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
                }

                // Kiểm tra IdsanphamNavigation có phải là null không
                if (gioHang.IdsanphamNavigation == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Sản phẩm với ID {request.IdSanPham} không tồn tại hoặc không có thông tin chi tiết."
                    });
                }


                // Cập nhật số lượng
                gioHang.Soluong += request.ThayDoi;

                // Nếu số lượng <= 0 thì xóa sản phẩm khỏi giỏ hàng
                if (gioHang.Soluong <= 0)
                {
                    _context.GioHangs.Remove(gioHang);
                }
                else
                {
                    _context.GioHangs.Update(gioHang);
                }

                _context.SaveChanges();

                // Tính toán lại thành tiền và tổng thanh toán
                var thanhTienMoi = gioHang.Soluong * gioHang.IdsanphamNavigation.Gia;
                var tongThanhTien = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == defaultUserId)
                    .Include(gh => gh.IdsanphamNavigation) // Nạp thông tin sản phẩm
                    .Sum(gh => gh.Soluong * gh.IdsanphamNavigation.Gia);

                return Json(new
                {
                    success = true,
                    soLuongMoi = gioHang.Soluong,
                    thanhTienMoi = thanhTienMoi,
                    tongThanhTien = tongThanhTien
                });
            }
            catch (Exception ex)
            {
                // In ra lỗi chi tiết để debug dễ dàng hơn
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }








        /*-----------------THANH TOÁN---------------*/
        public IActionResult ThanhToan()
        {
            int userId = 1; // Thay bằng Idnguoidung thực tế của người dùng hiện tại

            // Lấy danh sách giỏ hàng kiểu CartItem
            var cartItems = _context.GioHangs
                .Where(gh => gh.Idnguoidung == userId)
                .Include(gh => gh.IdsanphamNavigation)
                .ToList();

            // Chuyển đổi CartItem thành GioHangViewModel
            var gioHangViewModels = cartItems.Select(gh => new GioHangViewModel
            {
                TenSanPham = gh.IdsanphamNavigation.Tensp,
                Gia = gh.IdsanphamNavigation.Gia,
                DiaChiAnh = gh.IdsanphamNavigation.DiachianhSp,
                SoLuong = gh.Soluong,
                ThanhTien = gh.Soluong * gh.IdsanphamNavigation.Gia
            }).ToList();

            // Tính tổng thành tiền và số lượng sản phẩm
            decimal tongThanhTien = gioHangViewModels.Sum(g => g.ThanhTien);
            int tongSoLuong = gioHangViewModels.Sum(g => g.SoLuong);


            // Truyền dữ liệu xuống View
            ViewBag.TongThanhTien = tongThanhTien;
            ViewBag.TongSoLuong = tongSoLuong;

            return View(gioHangViewModels);
        }

        public IActionResult DatHangThanhCong()
        {
            return View();
        }

        public IActionResult CapNhatThongTin()
        {
            return View();
        }

        public IActionResult LichSuDonHang()
        {
            return View();
        }

        public IActionResult ChiTietDonHang()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    ProductName = "Canon EOS 7D Mark II",
                    Price = 42800000,
                    Quantity = 1,
                    ImageUrl = "./../images/canon_camera.png"
                },
                new CartItem
                {
                    ProductName = "Fujifilm Instax Mini 11 (Ice White)",
                    Price = 990000,
                    Quantity = 1,
                    ImageUrl = "./../images/instax_camera.png"
                },

                new CartItem
                {
                    ProductName = "Fujifilm Instax Mini 11 (Ice White)",
                    Price = 990000,
                    Quantity = 2,
                    ImageUrl = "./../images/instax_camera.png"
                }


            };
            return View(cartItems);
        }

        public IActionResult DiaChiNhanHang()
        {
            return View();
        }

        public IActionResult DoiMatKhau()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CapNhatSoLuongRequest
    {
        public int IdSanPham { get; set; }
        public int ThayDoi { get; set; } // +1 hoặc -1
    }

}


