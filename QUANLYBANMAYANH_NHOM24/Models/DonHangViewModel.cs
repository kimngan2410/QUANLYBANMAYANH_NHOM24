namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class DonHangViewModel
    {
        public int IdDonHang { get; set; }
        public DateTime? NgayDat { get; set; }  // Keep this as DateTime? (nullable DateTime)
        public string TenNguoiNhan { get; set; }
        public string DiaChi { get; set; }
        public string SdtNguoiNhan { get; set; }
        public string TrangThai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public List<SanPhamViewModel> SanPham { get; set; }
    }
}
