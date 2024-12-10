using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QUANLYBANMAYANH_NHOM24.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QUANLYBANMAYANH_NHOM24.Utilities;
using Azure.Core;


namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly QuanLyBanMayAnhContext _context;
        private readonly ViewRenderHelper _viewRenderHelper;

        public KhachHangController(QuanLyBanMayAnhContext context, ViewRenderHelper viewRenderHelper)
        {
            _context = context;
            _viewRenderHelper = viewRenderHelper; // Gán ViewRenderHelper vào biến _viewRenderHelper
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
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung"); // Lấy ID người dùng từ session

            if (idNguoiDung == null) // Nếu chưa đăng nhập
            {
                // Trả về Partial View đăng nhập
                var partialHtml = _viewRenderHelper.RenderPartialViewToString(this, "_LoginPartial", new LoginViewModel());


                return Json(new
                {
                    success = false,
                    message = "Bạn cần đăng nhập trước khi thêm sản phẩm vào giỏ hàng.",
                    partialLoginHtml = partialHtml // Gửi HTML của Partial View xuống client
                });
            }

            // Nếu đã đăng nhập, xử lý thêm sản phẩm vào giỏ hàng
            var gioHang = _context.GioHangs.FirstOrDefault(gh => gh.Idnguoidung == idNguoiDung && gh.Idsanpham == idSanPham);

            if (gioHang != null)
            {
                gioHang.Soluong += soLuong;
                _context.GioHangs.Update(gioHang);
            }
            else
            {
                var newGioHang = new GioHang
                {
                    Idnguoidung = idNguoiDung.Value, // Gắn sản phẩm vào tài khoản hiện tại
                    Idsanpham = idSanPham,
                    Soluong = soLuong
                };
                _context.GioHangs.Add(newGioHang);
            }

            _context.SaveChanges();

            // Cập nhật số lượng giỏ hàng
            int cartCount = _context.GioHangs.Where(gh => gh.Idnguoidung == idNguoiDung).Sum(gh => gh.Soluong);

            return Json(new { success = true, cartCount = cartCount });
        }




        /*--- Cập nhật số lượng sản phẩm trong giỏ hàng ---*/
        [HttpGet]
        public IActionResult LaySoLuongGioHang()
        {
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

            if (idNguoiDung == null)
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });
            }

            int cartCount = _context.GioHangs.Where(gh => gh.Idnguoidung == idNguoiDung).Sum(gh => gh.Soluong);

            return Json(new { success = true, cartCount = cartCount });
        }


        public IActionResult GioHang_2()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung"); // Lấy ID người dùng từ session
            var gioHang = _context.GioHangs
                .Where(gh => gh.Idnguoidung == idNguoiDung)
                .Include(gh => gh.IdsanphamNavigation)
                .Select(gh => new GioHangViewModel
                {
                    Idgiohang = gh.IdGioHang,
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
            ViewBag.IdNguoiDung = idNguoiDung;  // Truyền IdNguoiDung qua ViewBag

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

                // Kiểm tra xem người dùng đã đăng nhập hay chưa
                int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung"); // Lấy ID người dùng từ session
                if (idNguoiDung == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để cập nhật giỏ hàng." });
                }

                // Tìm sản phẩm trong giỏ hàng
                var gioHang = _context.GioHangs
                    .Include(gh => gh.IdsanphamNavigation) // Nạp thông tin sản phẩm
                    .FirstOrDefault(gh => gh.Idnguoidung == idNguoiDung && gh.Idsanpham == request.IdSanPham);

                if (gioHang == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
                }

                // Kiểm tra thông tin sản phẩm
                if (gioHang.IdsanphamNavigation == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Sản phẩm với ID {request.IdSanPham} không tồn tại hoặc không có thông tin chi tiết."
                    });
                }

                // Cập nhật số lượng sản phẩm
                gioHang.Soluong += request.ThayDoi;

                if (gioHang.Soluong <= 0)
                {
                    // Nếu số lượng <= 0 thì xóa sản phẩm khỏi giỏ hàng
                    _context.GioHangs.Remove(gioHang);
                }
                else
                {
                    // Cập nhật lại giỏ hàng mà không tạo mới IdGioHang
                    _context.GioHangs.Update(gioHang);
                }

                _context.SaveChanges();

                // Tính toán lại thành tiền và tổng thanh toán
                var thanhTienMoi = gioHang.Soluong * gioHang.IdsanphamNavigation.Gia;
                var tongThanhTien = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == idNguoiDung)
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
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }




        [HttpPost]
        public IActionResult XoaSanPham(int idSanPham)
        {
            try
            {
                var idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

                if (idNguoiDung == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện thao tác này." });
                }

                // Tìm sản phẩm trong giỏ hàng của người dùng
                var gioHang = _context.GioHangs
                    .FirstOrDefault(gh => gh.Idnguoidung == idNguoiDung && gh.Idsanpham == idSanPham);

                if (gioHang != null)
                {
                    _context.GioHangs.Remove(gioHang);
                    _context.SaveChanges(); // Xóa sản phẩm khỏi cơ sở dữ liệu
                }

                // Cập nhật lại tổng số lượng và tổng tiền
                var tongSoLuong = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == idNguoiDung)
                    .Sum(gh => gh.Soluong);

                var tongThanhTien = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == idNguoiDung)
                    .Sum(gh => gh.Soluong * gh.IdsanphamNavigation.Gia);

                // Kiểm tra nếu giỏ hàng trống và chuyển hướng
                if (tongSoLuong == 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Giỏ hàng của bạn đã trống.",
                        tongSoLuong = tongSoLuong,
                        tongThanhTien = tongThanhTien
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Sản phẩm đã được xóa khỏi giỏ hàng.",
                    tongSoLuong = tongSoLuong,
                    tongThanhTien = tongThanhTien
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


















        /*-------------------------------------THANH TOÁN-----------------------------------*/

        [HttpGet]
        public IActionResult ThanhToan()
        {
            var idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

            if (idNguoiDung != null)
            {
                ViewBag.IdNguoiDung = idNguoiDung; // Truyền giá trị vào ViewBag
            }

            // Lấy danh sách giỏ hàng
            var cartItems = _context.GioHangs
                .Where(gh => gh.Idnguoidung == idNguoiDung)
                .Include(gh => gh.IdsanphamNavigation)
                .ToList();

            var gioHangViewModels = cartItems.Select(gh => new GioHangViewModel
            {
                IdSanPham = gh.Idsanpham, // Lấy IdSanPham từ giỏ hàng
                TenSanPham = gh.IdsanphamNavigation.Tensp,
                Gia = gh.IdsanphamNavigation.Gia,
                DiaChiAnh = gh.IdsanphamNavigation.DiachianhSp,
                SoLuong = gh.Soluong,
                ThanhTien = gh.Soluong * gh.IdsanphamNavigation.Gia
            }).ToList();

            // Lấy danh sách phương thức thanh toán
            var phuongThucThanhToanList = _context.PhuongThucThanhToans.ToList();

            // Tạo đối tượng ThanhToanViewModel
            var viewModel = new ThanhToanViewModel
            {
                Name = "Tên người dùng", // Tạm thời
                Phone = "0123456789",    // Tạm thời
                Address = "Địa chỉ người dùng", // Tạm thời
                Note = "Ghi chú",
                PaymentMethodId = phuongThucThanhToanList.FirstOrDefault()?.IdPttt ?? 0,  // Lấy phương thức thanh toán mặc định
                GioHang = gioHangViewModels, // Khởi tạo danh sách giỏ hàng
                UserId = idNguoiDung ?? 0
            };

            // Truyền dữ liệu xuống View
            ViewBag.TongThanhTien = gioHangViewModels.Sum(g => g.ThanhTien);
            ViewBag.TongSoLuong = gioHangViewModels.Sum(g => g.SoLuong);

            return View(viewModel);
        }





        [HttpPost]
        public async Task<IActionResult> ThanhToan(ThanhToanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả về lại form với lỗi
                return View(model);
            }

         

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Lưu địa chỉ giao hàng
                var diaChi = new DiaChiGiaoHang
                {
                    Idnguoidung = model.UserId, // Lấy từ session hoặc user login
                    Tennguoinhan = model.Name,
                    Sdtnguoinhan = model.Phone,
                    Diachi = model.Address
                };
                _context.DiaChiGiaoHangs.Add(diaChi);
                await _context.SaveChangesAsync();

                // 2. Lưu thông tin đơn hàng
                var donHang = new DonHang
                {
                    Idnguoidung = model.UserId,
                    Ngaydat = DateTime.Now,
                    Iddiachi = diaChi.Iddiachi,
                    IdPttt = model.PaymentMethodId, // Sử dụng giá trị phương thức thanh toán
                    Trangthai = "Đang xử lý"
                };
                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync(); // Lưu đơn hàng để có Iddonhang

                // 3. Lưu chi tiết đơn hàng và xóa sản phẩm khỏi giỏ hàng
                var cartItems = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == model.UserId)
                    .Include(gh => gh.IdsanphamNavigation)  // Bao gồm thông tin của sản phẩm
                    .ToList();
                foreach (var item in cartItems)
                {
                    // Thêm vào chi tiết đơn hàng
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        Iddonhang = donHang?.Iddonhang ?? 0, // Giá trị mặc định nếu donHang là null
                        Idsanpham = item?.Idsanpham ?? 0, // Giá trị mặc định nếu item là null
                        Soluong = item?.Soluong ?? 0, // Giá trị mặc định nếu item là null
                        UnitPrice = item?.IdsanphamNavigation?.Gia ?? 0 // Giá từ bảng SanPham (IdsanphamNavigation)
                    };

                    _context.ChiTietDonHangs.Add(chiTietDonHang); // Thêm chi tiết đơn hàng

                    // Xóa sản phẩm khỏi giỏ hàng
                    _context.GioHangs.Remove(item);
                }

                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                // 4. Hoàn tất giao dịch
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Thanh toán thành công!";
                return RedirectToAction("DatHangThanhCong", new { idNguoiDung = donHang.Idnguoidung });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra: " + ex.Message);
                return View(model);
            }
        }







        public async Task<IActionResult> DatHangThanhCong(int idNguoiDung)
        {
            var donHang = await _context.DonHangs
                .Where(dh => dh.Idnguoidung == idNguoiDung)
                .OrderByDescending(dh => dh.Ngaydat)
                .Select(dh => new
                {
                    dh.Iddonhang,
                    dh.Ngaydat,
                    TotalAmount = dh.ChiTietDonHangs.Sum(ct => ct.Soluong * ct.UnitPrice),
                    PaymentMethod = dh.IdPtttNavigation.Tenphuongthuc
                })
                .FirstOrDefaultAsync();

            // Truyền idNguoiDung vào ViewBag
            ViewBag.IdNguoiDung = idNguoiDung;

            if (donHang == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            // Tạo ViewModel hoặc trả dữ liệu sang View
            var viewModel = new DonHangViewModel
            {
                MaDonHang = donHang.Iddonhang,
                NgayMua = donHang.Ngaydat,
                TongTien = donHang.TotalAmount,
                PhuongThucThanhToan = donHang.PaymentMethod
            };

            return View(viewModel);
        }




        public IActionResult CapNhatThongTin()
        {
            return View();
        }





        /*--------------------------ĐƠN HÀNG--------------------------------*/

        public async Task<IActionResult> LichSuDonHang()
        {
            // Lấy idNguoiDung từ Session
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung"); // Lấy ID người dùng từ session


            // Truy vấn danh sách đơn hàng
            var donHangs = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .Where(dh => dh.Idnguoidung == idNguoiDung.Value)
                .OrderByDescending(dh => dh.Ngaydat)
                .Select(dh => new DonHangViewModel
                {
                    MaDonHang = dh.Iddonhang,
                    NgayMua = dh.Ngaydat,
                    TongTien = dh.ChiTietDonHangs.Sum(ct => ct.Soluong * ct.UnitPrice),
                    Trangthai = dh.Trangthai
                })
                .ToListAsync();

            Console.WriteLine($"Số đơn hàng: {donHangs.Count}");

            return View(donHangs);
        }




        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            // Lấy idNguoiDung từ Session
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

            if (idNguoiDung == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện thao tác này." });
            }

            // Truy vấn chi tiết đơn hàng
            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.IdsanphamNavigation) // Lấy thông tin sản phẩm
                .Include(dh => dh.IdPtttNavigation) // Lấy phương thức thanh toán
                .Where(dh => dh.Iddonhang == id && dh.Idnguoidung == idNguoiDung.Value)
                .Select(dh => new DonHangChiTietViewModel
                {
                    MaDonHang = dh.Iddonhang,
                    NgayMua = dh.Ngaydat,
                    Trangthai = dh.Trangthai,
                    PhuongThucThanhToan = dh.IdPtttNavigation.Tenphuongthuc,
                    TongTien = dh.ChiTietDonHangs.Sum(ct => ct.Soluong * ct.UnitPrice),
                    ChiTietDonHangs = dh.ChiTietDonHangs.Select(ct => new ChiTietDonHangViewModel
                    {
                        TenSanPham = ct.IdsanphamNavigation.Tensp,
                        HinhAnh = ct.IdsanphamNavigation.DiachianhSp,
                        DonGia = ct.UnitPrice,
                        SoLuong = ct.Soluong,
                        ThanhTien = ct.Soluong * ct.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
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







    /*-------------------------------- CÁC REQUEST ----------------------------------*/

    public class CapNhatSoLuongRequest
    {
        public int IdSanPham { get; set; }
        public int ThayDoi { get; set; } // +1 hoặc -1
    }


    public class CheckoutRequest
    {
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public int PhuongThucThanhToan { get; set; } // ID phương thức thanh toán
        public List<GioHangViewModel> SanPhamDaMua { get; set; }
    }


}


