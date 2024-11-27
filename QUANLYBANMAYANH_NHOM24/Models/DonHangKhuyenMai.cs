using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class DonHangKhuyenMai
{
    public int IddonhangKm { get; set; }

    public int Iddonhang { get; set; }

    public int Idkhuyenmai { get; set; }

    public virtual DonHang IddonhangNavigation { get; set; } = null!;

    public virtual KhuyenMai IdkhuyenmaiNavigation { get; set; } = null!;
}
