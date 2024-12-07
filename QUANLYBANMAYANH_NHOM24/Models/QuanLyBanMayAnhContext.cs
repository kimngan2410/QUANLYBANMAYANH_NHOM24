using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QUANLYBANMAYANH_NHOM24.Models;

public partial class QuanLyBanMayAnhContext : DbContext
{
    public QuanLyBanMayAnhContext()
    {
    }

    public QuanLyBanMayAnhContext(DbContextOptions<QuanLyBanMayAnhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DanhMucCon> DanhMucCons { get; set; }

    public virtual DbSet<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<DonHangKhuyenMai> DonHangKhuyenMais { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<Hang> Hangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NguoiDungPhanQuyen> NguoiDungPhanQuyens { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.Idchitietdonhang).HasName("PK__ChiTietD__D719A734F7B70079");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.Idchitietdonhang).HasColumnName("idchitietdonhang");
            entity.Property(e => e.Iddonhang).HasColumnName("iddonhang");
            entity.Property(e => e.Idsanpham).HasColumnName("idsanpham");
            entity.Property(e => e.Soluong).HasColumnName("soluong");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.IddonhangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.Iddonhang)
                .HasConstraintName("FK__ChiTietDo__iddon__5629CD9C");

            entity.HasOne(d => d.IdsanphamNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.Idsanpham)
                .HasConstraintName("FK__ChiTietDo__idsan__571DF1D5");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.Iddanhmuc).HasName("PK__DanhMuc__F4CD32E37138F45C");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.Iddanhmuc).HasColumnName("iddanhmuc");
            entity.Property(e => e.Tendanhmuc)
                .HasMaxLength(100)
                .HasColumnName("tendanhmuc");

        });

        modelBuilder.Entity<DanhMucCon>(entity =>
        {
            entity.HasKey(e => e.Iddanhmuccon).HasName("PK__DanhMucC__CE970A9A06C7B2DB");

            entity.ToTable("DanhMucCon");

            entity.Property(e => e.Iddanhmuccon).HasColumnName("iddanhmuccon");
            entity.Property(e => e.Iddanhmuc).HasColumnName("iddanhmuc");
            entity.Property(e => e.Tendanhmuccon)
                .HasMaxLength(100)
                .HasColumnName("tendanhmuccon");

            entity.HasOne(d => d.IddanhmucNavigation).WithMany(p => p.DanhMucCons)
                .HasForeignKey(d => d.Iddanhmuc)
                .HasConstraintName("FK__DanhMucCo__iddan__3C69FB99");
        });

        modelBuilder.Entity<DiaChiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.Iddiachi).HasName("PK__DiaChiGi__A8D1197358181C2E");

            entity.ToTable("DiaChiGiaoHang");

            entity.Property(e => e.Iddiachi).HasColumnName("iddiachi");
            entity.Property(e => e.Diachi).HasColumnName("diachi");
            entity.Property(e => e.Idnguoidung).HasColumnName("idnguoidung");
            entity.Property(e => e.Sdtnguoinhan)
                .HasMaxLength(15)
                .HasColumnName("SDTnguoinhan");
            entity.Property(e => e.Tennguoinhan)
                .HasMaxLength(100)
                .HasColumnName("tennguoinhan");

            entity.HasOne(d => d.IdnguoidungNavigation).WithMany(p => p.DiaChiGiaoHangs)
                .HasForeignKey(d => d.Idnguoidung)
                .HasConstraintName("FK__DiaChiGia__idngu__46E78A0C");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.Iddonhang).HasName("PK__DonHang__6DCE9B0043CEA171");

            entity.ToTable("DonHang");

            entity.Property(e => e.Iddonhang).HasColumnName("iddonhang");
            entity.Property(e => e.IdPttt).HasColumnName("idPTTT");
            entity.Property(e => e.Iddiachi).HasColumnName("iddiachi");
            entity.Property(e => e.Idnguoidung).HasColumnName("idnguoidung");
            entity.Property(e => e.Ngaydat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngaydat");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang xử lý")
                .HasColumnName("trangthai");

            entity.HasOne(d => d.IdPtttNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.IdPttt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__idPTTT__534D60F1");

            entity.HasOne(d => d.IddiachiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.Iddiachi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__iddiach__52593CB8");

            entity.HasOne(d => d.IdnguoidungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.Idnguoidung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__idnguoi__5165187F");
        });

        modelBuilder.Entity<DonHangKhuyenMai>(entity =>
        {
            entity.HasKey(e => e.IddonhangKm).HasName("PK__DonHangK__B719AEBCBE29CEE4");

            entity.ToTable("DonHangKhuyenMai");

            entity.Property(e => e.IddonhangKm).HasColumnName("iddonhangKM");
            entity.Property(e => e.Iddonhang).HasColumnName("iddonhang");
            entity.Property(e => e.Idkhuyenmai).HasColumnName("idkhuyenmai");

            entity.HasOne(d => d.IddonhangNavigation).WithMany(p => p.DonHangKhuyenMais)
                .HasForeignKey(d => d.Iddonhang)
                .HasConstraintName("FK__DonHangKh__iddon__60A75C0F");

            entity.HasOne(d => d.IdkhuyenmaiNavigation).WithMany(p => p.DonHangKhuyenMais)
                .HasForeignKey(d => d.Idkhuyenmai)
                .HasConstraintName("FK__DonHangKh__idkhu__619B8048");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.IdGioHang).HasName("PK__GioHang__3FD868429D998115");

            entity.ToTable("GioHang");

            entity.Property(e => e.IdGioHang).HasColumnName("idGioHang");
            entity.Property(e => e.Idnguoidung).HasColumnName("idnguoidung");
            entity.Property(e => e.Idsanpham).HasColumnName("idsanpham");
            entity.Property(e => e.Soluong).HasColumnName("soluong");

            entity.HasOne(d => d.IdnguoidungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.Idnguoidung)
                .HasConstraintName("FK__GioHang__idnguoi__59FA5E80");

            entity.HasOne(d => d.IdsanphamNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.Idsanpham)
                .HasConstraintName("FK__GioHang__idsanph__5AEE82B9");
        });

        modelBuilder.Entity<Hang>(entity =>
        {
            entity.HasKey(e => e.Idhang).HasName("PK__Hang__ABCF290EBA8DEC53");

            entity.ToTable("Hang");

            entity.Property(e => e.Idhang).HasColumnName("idhang");
            entity.Property(e => e.Tenhang)
                .HasMaxLength(100)
                .HasColumnName("tenhang");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.Idkhuyenmai).HasName("PK__KhuyenMa__59172652E4D44434");

            entity.ToTable("KhuyenMai");

            entity.HasIndex(e => e.Makhuyenmai, "UQ__KhuyenMa__77F420CC6335069A").IsUnique();

            entity.Property(e => e.Idkhuyenmai).HasColumnName("idkhuyenmai");
            entity.Property(e => e.Giamgiaphantram).HasColumnName("giamgiaphantram");
            entity.Property(e => e.Makhuyenmai)
                .HasMaxLength(20)
                .HasColumnName("makhuyenmai");
            entity.Property(e => e.Ngaybatdau).HasColumnName("ngaybatdau");
            entity.Property(e => e.Ngayhethan).HasColumnName("ngayhethan");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.Idnguoidung).HasName("PK__NguoiDun__7C5001957141FBC3");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__AB6E6164ACB2545F").IsUnique();

            entity.Property(e => e.Idnguoidung).HasColumnName("idnguoidung");
            entity.Property(e => e.AnhNguoiDung).HasColumnName("anh_nguoi_dung");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(10)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(255)
                .HasColumnName("matkhau");
            entity.Property(e => e.Ngaysinh).HasColumnName("ngaysinh");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(50)
                .HasColumnName("ten_nguoi_dung");
        });

        modelBuilder.Entity<NguoiDungPhanQuyen>(entity =>
        {
            entity.HasKey(e => e.IdNdpq).HasName("PK__NguoiDun__C23E414D02ED019F");

            entity.ToTable("NguoiDungPhanQuyen");

            entity.Property(e => e.IdNdpq).HasColumnName("idNDPQ");
            entity.Property(e => e.Idnguoidung).HasColumnName("idnguoidung");
            entity.Property(e => e.Idphanquyen).HasColumnName("idphanquyen");

            entity.HasOne(d => d.IdnguoidungNavigation).WithMany(p => p.NguoiDungPhanQuyens)
                .HasForeignKey(d => d.Idnguoidung)
                .HasConstraintName("FK__NguoiDung__idngu__6754599E");

            entity.HasOne(d => d.IdphanquyenNavigation).WithMany(p => p.NguoiDungPhanQuyens)
                .HasForeignKey(d => d.Idphanquyen)
                .HasConstraintName("FK__NguoiDung__idpha__68487DD7");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.Idphanquyen).HasName("PK__PhanQuye__23C0CE5F990D7A0B");

            entity.ToTable("PhanQuyen");

            entity.HasIndex(e => e.TenPq, "UQ__PhanQuye__FB74F8710F305027").IsUnique();

            entity.Property(e => e.Idphanquyen).HasColumnName("idphanquyen");
            entity.Property(e => e.TenPq)
                .HasMaxLength(50)
                .HasColumnName("tenPQ");
        });

        modelBuilder.Entity<PhuongThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.IdPttt).HasName("PK__PhuongTh__DE0378267CCB0FE1");

            entity.ToTable("PhuongThucThanhToan");

            entity.Property(e => e.IdPttt).HasColumnName("idPTTT");
            entity.Property(e => e.Tenphuongthuc)
                .HasMaxLength(50)
                .HasColumnName("tenphuongthuc");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Idsanpham).HasName("PK__SanPham__C5FDB0E5641EF42A");

            entity.ToTable("SanPham");

            entity.Property(e => e.Idsanpham).HasColumnName("idsanpham");
            entity.Property(e => e.DiachianhSp).HasColumnName("diachianhSP");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("gia");
            entity.Property(e => e.Iddanhmuccon).HasColumnName("iddanhmuccon");
            entity.Property(e => e.Idhang).HasColumnName("idhang");
            entity.Property(e => e.Mota).HasColumnName("mota");
            entity.Property(e => e.Soluongcon).HasColumnName("soluongcon");
            entity.Property(e => e.Tensp)
                .HasMaxLength(100)
                .HasColumnName("tensp");

            entity.HasOne(d => d.IddanhmucconNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.Iddanhmuccon)
                .HasConstraintName("FK__SanPham__iddanhm__412EB0B6");

            entity.HasOne(d => d.IdhangNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.Idhang)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SanPham__idhang__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
