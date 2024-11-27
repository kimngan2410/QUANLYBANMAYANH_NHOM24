using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class NguoiDungPhanQuyen
{
    public int IdNdpq { get; set; }

    public int Idnguoidung { get; set; }

    public int Idphanquyen { get; set; }

    public virtual NguoiDung IdnguoidungNavigation { get; set; } = null!;

    public virtual PhanQuyen IdphanquyenNavigation { get; set; } = null!;
}
