using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class ChiTietDonHang
{
    public int Idchitietdonhang { get; set; }

    public int Iddonhang { get; set; }

    public int Idsanpham { get; set; }

    public int Soluong { get; set; }

    public decimal UnitPrice { get; set; }

    // Define the ThanhTien property as a calculated value

    public virtual DonHang IddonhangNavigation { get; set; } = null!;

    public virtual SanPham IdsanphamNavigation { get; set; } = null!;
}
