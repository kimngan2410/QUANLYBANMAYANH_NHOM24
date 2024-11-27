using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class DonHang
{
    public int Iddonhang { get; set; }

    public int Idnguoidung { get; set; }

    public DateTime? Ngaydat { get; set; }

    public int Iddiachi { get; set; }

    public int IdPttt { get; set; }

    public string Trangthai { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DonHangKhuyenMai> DonHangKhuyenMais { get; set; } = new List<DonHangKhuyenMai>();

    public virtual PhuongThucThanhToan IdPtttNavigation { get; set; } = null!;

    public virtual DiaChiGiaoHang IddiachiNavigation { get; set; } = null!;

    public virtual NguoiDung IdnguoidungNavigation { get; set; } = null!;
}
