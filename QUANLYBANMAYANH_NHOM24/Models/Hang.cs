using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class Hang
{
    public int Idhang { get; set; }

    public string Tenhang { get; set; } = null!;

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
