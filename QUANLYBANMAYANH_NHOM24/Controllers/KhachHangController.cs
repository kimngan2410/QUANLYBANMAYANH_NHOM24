using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QUANLYBANMAYANH_NHOM24.Models;


namespace QUANLYBANMAYANH_NHOM24.Controllers
{
    public class KhachHangController : Controller
    {
       
        public IActionResult GioHang()
        {
            return View();
        }

        public IActionResult GioHang_2()
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
                    Quantity = 1,
                    ImageUrl = "./../images/instax_camera.png"
                }


            };
            return View(cartItems);
        }

        public IActionResult ThanhToan()
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
    }

    public class CartItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
