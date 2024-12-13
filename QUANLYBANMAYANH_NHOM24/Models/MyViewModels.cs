namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class MyViewModels
    {
        public IEnumerable<DonHangViewModel> DonHang { get; set; }
        public IEnumerable<ChiTietDonHangViewModel> chiTietDonHang { get; set; }

        public NguoiDung NguoiDung { get; set; }

        // Change this to IEnumerable to handle multiple shipping addresses
        public IEnumerable<DiaChiGiaoHang> DiaChiGiaoHang { get; set; }

        public DonHangChiTietViewModel DonHangChiTiet { get; set; }
    }
}
