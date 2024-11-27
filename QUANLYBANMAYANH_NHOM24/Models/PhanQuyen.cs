using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class PhanQuyen
{
    public int Idphanquyen { get; set; }

    public string TenPq { get; set; } = null!;

    public virtual ICollection<NguoiDungPhanQuyen> NguoiDungPhanQuyens { get; set; } = new List<NguoiDungPhanQuyen>();
}
