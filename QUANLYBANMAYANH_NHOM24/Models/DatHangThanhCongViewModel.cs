namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class DatHangThanhCongViewModel
    {
        public int MaDonHang { get; set; }
        public string NgayMua { get; set; }
        public decimal TongTien { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public List<ChiTietSanPhamViewModel> ChiTietSanPham { get; set; }
    }
}
