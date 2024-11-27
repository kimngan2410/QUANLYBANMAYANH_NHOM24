using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class DanhMuc
{
    public int Iddanhmuc { get; set; }

    public string Tendanhmuc { get; set; } = null!;

    public virtual ICollection<DanhMucCon> DanhMucCons { get; set; } = new List<DanhMucCon>();
}
