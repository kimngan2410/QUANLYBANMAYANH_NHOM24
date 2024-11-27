using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class GioHang
{
    public int IdGioHang { get; set; }

    public int Idnguoidung { get; set; }

    public int Idsanpham { get; set; }

    public int Soluong { get; set; }

    public virtual NguoiDung IdnguoidungNavigation { get; set; } = null!;

    public virtual SanPham IdsanphamNavigation { get; set; } = null!;
}
