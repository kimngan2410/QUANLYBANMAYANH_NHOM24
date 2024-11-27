using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class DiaChiGiaoHang
{
    public int Iddiachi { get; set; }

    public int Idnguoidung { get; set; }

    public string Tennguoinhan { get; set; } = null!;

    public string Sdtnguoinhan { get; set; } = null!;

    public string Diachi { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual NguoiDung IdnguoidungNavigation { get; set; } = null!;
}
