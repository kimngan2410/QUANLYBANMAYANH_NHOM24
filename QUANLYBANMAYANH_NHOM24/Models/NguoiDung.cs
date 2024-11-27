using System;
using System.Collections.Generic;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class NguoiDung
{
    public int Idnguoidung { get; set; }

    public string TenNguoiDung { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Sdt { get; set; }

    public string Matkhau { get; set; } = null!;

    public string? Gioitinh { get; set; }

    public DateOnly? Ngaysinh { get; set; }

    public string? AnhNguoiDung { get; set; }

    public virtual ICollection<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<NguoiDungPhanQuyen> NguoiDungPhanQuyens { get; set; } = new List<NguoiDungPhanQuyen>();
}
