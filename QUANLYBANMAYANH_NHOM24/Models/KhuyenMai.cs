using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class KhuyenMai
{
    public int Idkhuyenmai { get; set; }

    public string Makhuyenmai { get; set; } = null!;

    public int Giamgiaphantram { get; set; }

    public DateOnly Ngaybatdau { get; set; }

    public DateOnly Ngayhethan { get; set; }

    public virtual ICollection<DonHangKhuyenMai> DonHangKhuyenMais { get; set; } = new List<DonHangKhuyenMai>();
}
