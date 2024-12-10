namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class DonHangChiTietViewModel
    {
        public int MaDonHang { get; set; }
        public string NgayMua { get; set; }
        public string Trangthai { get; set; }
        public decimal TongTien { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public List<ChiTietDonHangViewModel> ChiTietDonHangs { get; set; }
    }
}
