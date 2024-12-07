namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class GioHangViewModel
    {
        public int IdGioHang { get; set; }
        public string TenSanPham { get; set; } = null!;
        public string? AnhSanPham { get; set; }
        public decimal GiaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => GiaSanPham * SoLuong;
    }
}
