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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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

            // Kiểm tra số lượng sản phẩm
            if (soLuong < 1 || soLuong > 18)
            {
                return Json(new { success = false, message = "Số lượng không hợp lệ." });
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

            // Kiểm tra xem người dùng đã có địa chỉ giao hàng chưa
            var diaChiGiaoHang = _context.DiaChiGiaoHangs
                .FirstOrDefault(d => d.Idnguoidung == idNguoiDung);

            // Tạo đối tượng ThanhToanViewModel
            var viewModel = new ThanhToanViewModel
            {
                Name = diaChiGiaoHang?.Tennguoinhan ?? "", // Nếu đã có địa chỉ, điền tên người nhận
                Phone = diaChiGiaoHang?.Sdtnguoinhan ?? "", // Nếu đã có địa chỉ, điền số điện thoại
                Address = diaChiGiaoHang?.Diachi ?? "", // Nếu đã có địa chỉ, điền địa chỉ
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
                DiaChiGiaoHang diaChi;

                // Kiểm tra xem người dùng có địa chỉ giao hàng chưa
                var existingDiaChi = _context.DiaChiGiaoHangs
                    .FirstOrDefault(d => d.Idnguoidung == model.UserId);

                if (existingDiaChi != null)
                {
                    // Nếu đã có địa chỉ, chỉ cần sử dụng địa chỉ đã có
                    diaChi = existingDiaChi;
                }
                else
                {
                    // Nếu chưa có địa chỉ, tạo mới địa chỉ giao hàng
                    diaChi = new DiaChiGiaoHang
                    {
                        Idnguoidung = model.UserId,
                        Tennguoinhan = model.Name,
                        Sdtnguoinhan = model.Phone,
                        Diachi = model.Address
                    };

                    _context.DiaChiGiaoHangs.Add(diaChi);
                    await _context.SaveChangesAsync();
                }

                // Lưu thông tin đơn hàng
                var donHang = new DonHang
                {
                    Idnguoidung = model.UserId,
                    Ngaydat = DateTime.Now,
                    Iddiachi = diaChi.Iddiachi,
                    IdPttt = model.PaymentMethodId,
                    Trangthai = "Đang xử lý"
                };
                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync(); // Lưu đơn hàng để có Iddonhang

                // Lưu chi tiết đơn hàng và xóa sản phẩm khỏi giỏ hàng
                var cartItems = _context.GioHangs
                    .Where(gh => gh.Idnguoidung == model.UserId)
                    .Include(gh => gh.IdsanphamNavigation)
                    .ToList();

                foreach (var item in cartItems)
                {
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        Iddonhang = donHang.Iddonhang,
                        Idsanpham = item.Idsanpham,
                        Soluong = item.Soluong,
                        UnitPrice = item.IdsanphamNavigation.Gia
                    };

                    _context.ChiTietDonHangs.Add(chiTietDonHang); // Thêm chi tiết đơn hàng

                    // Xóa sản phẩm khỏi giỏ hàng
                    _context.GioHangs.Remove(item);
                }

                await _context.SaveChangesAsync();

                // Hoàn tất giao dịch
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
                NgayMua = donHang.Ngaydat.HasValue ? donHang.Ngaydat.Value.ToString("dd/MMM/yyyy") : "Không xác định",
                TongTien = donHang.TotalAmount,
                PhuongThucThanhToan = donHang.PaymentMethod
            };

            return View(viewModel);
        }




        /*--------------------------ĐƠN HÀNG--------------------------------*/

        /*1. Danh sách đơn hàng*/
        [HttpGet]
        public async Task<IActionResult> LichSuDonHang(string query = "")
        {
            // Lấy idNguoiDung từ Session
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

            if (idNguoiDung == null)
            {
                return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển tới trang đăng nhập
            }

            // Truy vấn danh sách đơn hàng của người dùng
            var donHangs = _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs) // Load chi tiết đơn hàng
                .Where(dh => dh.Idnguoidung == idNguoiDung.Value) // Chỉ lấy đơn hàng của idNguoiDung
                .AsQueryable();

            // Kiểm tra điều kiện tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(query))
            {
                // Kiểm tra nếu query là ngày (dd/MM/yyyy)
                if (DateTime.TryParseExact(query, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    donHangs = donHangs.Where(d => d.Ngaydat.HasValue && d.Ngaydat.Value.Date == parsedDate.Date);
                }
                // Kiểm tra nếu query là mã đơn hàng (int)
                else if (int.TryParse(query, out int parsedId))
                {
                    donHangs = donHangs.Where(d => d.Iddonhang == parsedId);
                }
            }

            // Chuyển đổi sang ViewModel
            var donHangViewModels = await donHangs
                .OrderByDescending(dh => dh.Ngaydat) // Sắp xếp theo ngày đặt giảm dần
                .Select(dh => new DonHangViewModel
                {
                    MaDonHang = dh.Iddonhang,
                    NgayMua = dh.Ngaydat.HasValue ? dh.Ngaydat.Value.ToString("dd/MMM/yyyy") : "Không xác định",
                    TongTien = dh.ChiTietDonHangs.Sum(ct => ct.Soluong * ct.UnitPrice),
                    Trangthai = dh.Trangthai
                })
                .ToListAsync();

            var nguoiDung = await _context.NguoiDungs.FirstOrDefaultAsync(nd => nd.Idnguoidung == idNguoiDung);
            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }
            var viewModel = new MyViewModels
            {
                NguoiDung = nguoiDung,  // Ensure that NguoiDung is initialized properly
                DonHang = donHangViewModels
            };

            return View(viewModel); // Đúng

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
                    NgayMua = dh.Ngaydat.HasValue ? dh.Ngaydat.Value.ToString("dd/MMM/yyyy") : "Không xác định",
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
                return NotFound(); // Nếu không tìm thấy đơn hàng
            }

            // Truy vấn thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = await _context.NguoiDungs
                .Where(nd => nd.Idnguoidung == idNguoiDung.Value)
                .FirstOrDefaultAsync();

            if (nguoiDung == null)
            {
                return Json(new { success = false, message = "Người dùng không tồn tại." });
            }

            // Khởi tạo ViewModel chung và gán NguoiDung
            var viewModel = new MyViewModels
            {
                DonHang = new List<DonHangViewModel> { new DonHangViewModel
            {
                MaDonHang = donHang.MaDonHang,
                NgayMua = donHang.NgayMua,
                Trangthai = donHang.Trangthai,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TongTien = donHang.TongTien
            }},
                    chiTietDonHang = donHang.ChiTietDonHangs,
                    DonHangChiTiet = donHang,
                    NguoiDung = nguoiDung // Gán đối tượng người dùng
                };

            return View(viewModel); // Trả về ViewModel chung
        }












        /*------------------------------------ NGƯỜI DÙNG -----------------------------------*/
        public async Task<IActionResult> CapNhatThongTin()
        {
            // Lấy Id người dùng từ session
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");
            if (idNguoiDung == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect nếu chưa đăng nhập
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(nd => nd.Idnguoidung == idNguoiDung.Value);

            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return View(nguoiDung); // Truyền thông tin người dùng vào view
        }


        [HttpPost]
        public async Task<IActionResult> CapNhatThongTin([FromBody] NguoiDung model)
        {
            if (model.Idnguoidung == null)
            {
                return BadRequest(new { message = "Thiếu ID người dùng." });
            }

            var nguoiDung = await _context.NguoiDungs.FirstOrDefaultAsync(nd => nd.Idnguoidung == model.Idnguoidung);
            if (nguoiDung == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại." });
            }

            // Chỉ cập nhật các trường không null
            if (!string.IsNullOrEmpty(model.TenNguoiDung))
            {
                nguoiDung.TenNguoiDung = model.TenNguoiDung;
            }

            if (!string.IsNullOrEmpty(model.Sdt))
            {
                nguoiDung.Sdt = model.Sdt;
            }

            if (model.Ngaysinh != null)
            {
                nguoiDung.Ngaysinh = model.Ngaysinh;
            }

            if (!string.IsNullOrEmpty(model.Gioitinh))
            {
                nguoiDung.Gioitinh = model.Gioitinh;
            }

            // Không cho phép thay đổi email
            // nguoiDung.Email = model.Email; // Không cập nhật email

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công!" });
        }








        public async Task<IActionResult> DiaChiNhanHang()
        {
            // Lấy Id người dùng từ session
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");
            if (idNguoiDung == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect nếu chưa đăng nhập
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(nd => nd.Idnguoidung == idNguoiDung);

            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Lấy thông tin địa chỉ giao hàng của người dùng
            var diaChiGiaoHang = await _context.DiaChiGiaoHangs
                .Where(d => d.Idnguoidung == idNguoiDung.Value)
                .ToListAsync();

            // Tạo viewModel với thông tin người dùng và địa chỉ giao hàng
            var viewModel = new MyViewModels
            {
                NguoiDung = nguoiDung,
                DiaChiGiaoHang = diaChiGiaoHang
            };

            return View(viewModel); // Truyền thông tin vào view
        }



        public IActionResult DoiMatKhau()
        {
            return View();
        }

       
       




        public IActionResult Index()
        {
            return View();
        }



        /*-------------------------------- CHI TIẾT SẢN PHẨM ----------------------------------*/

        public IActionResult ChiTietSanPham(int id)
        {
            int? idNguoiDung = HttpContext.Session.GetInt32("IdNguoiDung");

           
            ViewBag.IdNguoiDung = idNguoiDung;

            var sanPham = _context.SanPhams
                .Include(sp => sp.IddanhmucconNavigation) // Lấy thông tin danh mục con nếu cần
                .Include(sp => sp.IdhangNavigation) // Lấy thông tin hãng
                .FirstOrDefault(sp => sp.Idsanpham == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }




        /*-------------------------------- GIỚI THIỆU ----------------------------------*/
        public IActionResult GioiThieu()
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


