using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class DanhMucCon
{
    public int Iddanhmuccon { get; set; }

    public string Tendanhmuccon { get; set; } = null!;

    public int Iddanhmuc { get; set; }

    public virtual DanhMuc IddanhmucNavigation { get; set; } = null!;

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
