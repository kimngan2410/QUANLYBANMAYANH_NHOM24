using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class SanPham
{
    public int Idsanpham { get; set; }

    public string Tensp { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? Mota { get; set; }

    public int Soluongcon { get; set; }

    public int Iddanhmuccon { get; set; }

    public int? Idhang { get; set; }

    public string? DiachianhSp { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual DanhMucCon IddanhmucconNavigation { get; set; } = null!;

    public virtual Hang? IdhangNavigation { get; set; }
}
