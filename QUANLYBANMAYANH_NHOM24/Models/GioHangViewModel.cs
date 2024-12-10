namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class GioHangViewModel
    {
        public int Idgiohang { get; set; }
        public int IdSanPham { get; set; } // ID của sản phẩm
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public string DiaChiAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }


    }
}
