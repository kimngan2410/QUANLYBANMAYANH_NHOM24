namespace QUANLYBANMAYANH_NHOM24.Models
{
    public class MyViewModels
    {
        public IEnumerable<DonHangViewModel> DonHang { get; set; }
        public IEnumerable<ChiTietDonHangViewModel> chiTietDonHang { get; set; }

        public NguoiDung NguoiDung { get; set; }
    }
}
