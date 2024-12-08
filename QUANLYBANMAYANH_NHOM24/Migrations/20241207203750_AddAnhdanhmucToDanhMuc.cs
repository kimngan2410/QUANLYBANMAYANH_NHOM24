using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QUANLYBANMAYANH_NHOM24.Migrations
{
    /// <inheritdoc />
    public partial class AddAnhdanhmucToDanhMuc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMuc",
                columns: table => new
                {
                    iddanhmuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tendanhmuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Anhdanhmuc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMuc__F4CD32E37138F45C", x => x.iddanhmuc);
                });

            migrationBuilder.CreateTable(
                name: "Hang",
                columns: table => new
                {
                    idhang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenhang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hang__ABCF290EBA8DEC53", x => x.idhang);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    idkhuyenmai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    makhuyenmai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    giamgiaphantram = table.Column<int>(type: "int", nullable: false),
                    ngaybatdau = table.Column<DateOnly>(type: "date", nullable: false),
                    ngayhethan = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhuyenMa__59172652E4D44434", x => x.idkhuyenmai);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    idnguoidung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_nguoi_dung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    matkhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    gioitinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ngaysinh = table.Column<DateOnly>(type: "date", nullable: true),
                    anh_nguoi_dung = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__7C5001957141FBC3", x => x.idnguoidung);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyen",
                columns: table => new
                {
                    idphanquyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenPQ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhanQuye__23C0CE5F990D7A0B", x => x.idphanquyen);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    idPTTT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenphuongthuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhuongTh__DE0378267CCB0FE1", x => x.idPTTT);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucCon",
                columns: table => new
                {
                    iddanhmuccon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tendanhmuccon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    iddanhmuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhMucC__CE970A9A06C7B2DB", x => x.iddanhmuccon);
                    table.ForeignKey(
                        name: "FK__DanhMucCo__iddan__3C69FB99",
                        column: x => x.iddanhmuc,
                        principalTable: "DanhMuc",
                        principalColumn: "iddanhmuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChiGiaoHang",
                columns: table => new
                {
                    iddiachi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idnguoidung = table.Column<int>(type: "int", nullable: false),
                    tennguoinhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDTnguoinhan = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    diachi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DiaChiGi__A8D1197358181C2E", x => x.iddiachi);
                    table.ForeignKey(
                        name: "FK__DiaChiGia__idngu__46E78A0C",
                        column: x => x.idnguoidung,
                        principalTable: "NguoiDung",
                        principalColumn: "idnguoidung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungPhanQuyen",
                columns: table => new
                {
                    idNDPQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idnguoidung = table.Column<int>(type: "int", nullable: false),
                    idphanquyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__C23E414D02ED019F", x => x.idNDPQ);
                    table.ForeignKey(
                        name: "FK__NguoiDung__idngu__6754599E",
                        column: x => x.idnguoidung,
                        principalTable: "NguoiDung",
                        principalColumn: "idnguoidung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__NguoiDung__idpha__68487DD7",
                        column: x => x.idphanquyen,
                        principalTable: "PhanQuyen",
                        principalColumn: "idphanquyen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    idsanpham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tensp = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    mota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soluongcon = table.Column<int>(type: "int", nullable: false),
                    iddanhmuccon = table.Column<int>(type: "int", nullable: false),
                    idhang = table.Column<int>(type: "int", nullable: true),
                    diachianhSP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPham__C5FDB0E5641EF42A", x => x.idsanpham);
                    table.ForeignKey(
                        name: "FK__SanPham__iddanhm__412EB0B6",
                        column: x => x.iddanhmuccon,
                        principalTable: "DanhMucCon",
                        principalColumn: "iddanhmuccon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__SanPham__idhang__4222D4EF",
                        column: x => x.idhang,
                        principalTable: "Hang",
                        principalColumn: "idhang",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    iddonhang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idnguoidung = table.Column<int>(type: "int", nullable: false),
                    ngaydat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    iddiachi = table.Column<int>(type: "int", nullable: false),
                    idPTTT = table.Column<int>(type: "int", nullable: false),
                    trangthai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Đang xử lý")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonHang__6DCE9B0043CEA171", x => x.iddonhang);
                    table.ForeignKey(
                        name: "FK__DonHang__idPTTT__534D60F1",
                        column: x => x.idPTTT,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "idPTTT");
                    table.ForeignKey(
                        name: "FK__DonHang__iddiach__52593CB8",
                        column: x => x.iddiachi,
                        principalTable: "DiaChiGiaoHang",
                        principalColumn: "iddiachi");
                    table.ForeignKey(
                        name: "FK__DonHang__idnguoi__5165187F",
                        column: x => x.idnguoidung,
                        principalTable: "NguoiDung",
                        principalColumn: "idnguoidung");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    idGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idnguoidung = table.Column<int>(type: "int", nullable: false),
                    idsanpham = table.Column<int>(type: "int", nullable: false),
                    soluong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GioHang__3FD868429D998115", x => x.idGioHang);
                    table.ForeignKey(
                        name: "FK__GioHang__idnguoi__59FA5E80",
                        column: x => x.idnguoidung,
                        principalTable: "NguoiDung",
                        principalColumn: "idnguoidung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__GioHang__idsanph__5AEE82B9",
                        column: x => x.idsanpham,
                        principalTable: "SanPham",
                        principalColumn: "idsanpham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    idchitietdonhang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iddonhang = table.Column<int>(type: "int", nullable: false),
                    idsanpham = table.Column<int>(type: "int", nullable: false),
                    soluong = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietD__D719A734F7B70079", x => x.idchitietdonhang);
                    table.ForeignKey(
                        name: "FK__ChiTietDo__iddon__5629CD9C",
                        column: x => x.iddonhang,
                        principalTable: "DonHang",
                        principalColumn: "iddonhang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietDo__idsan__571DF1D5",
                        column: x => x.idsanpham,
                        principalTable: "SanPham",
                        principalColumn: "idsanpham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangKhuyenMai",
                columns: table => new
                {
                    iddonhangKM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iddonhang = table.Column<int>(type: "int", nullable: false),
                    idkhuyenmai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonHangK__B719AEBCBE29CEE4", x => x.iddonhangKM);
                    table.ForeignKey(
                        name: "FK__DonHangKh__iddon__60A75C0F",
                        column: x => x.iddonhang,
                        principalTable: "DonHang",
                        principalColumn: "iddonhang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DonHangKh__idkhu__619B8048",
                        column: x => x.idkhuyenmai,
                        principalTable: "KhuyenMai",
                        principalColumn: "idkhuyenmai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_iddonhang",
                table: "ChiTietDonHang",
                column: "iddonhang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_idsanpham",
                table: "ChiTietDonHang",
                column: "idsanpham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucCon_iddanhmuc",
                table: "DanhMucCon",
                column: "iddanhmuc");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiGiaoHang_idnguoidung",
                table: "DiaChiGiaoHang",
                column: "idnguoidung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_iddiachi",
                table: "DonHang",
                column: "iddiachi");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_idnguoidung",
                table: "DonHang",
                column: "idnguoidung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_idPTTT",
                table: "DonHang",
                column: "idPTTT");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangKhuyenMai_iddonhang",
                table: "DonHangKhuyenMai",
                column: "iddonhang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangKhuyenMai_idkhuyenmai",
                table: "DonHangKhuyenMai",
                column: "idkhuyenmai");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_idnguoidung",
                table: "GioHang",
                column: "idnguoidung");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_idsanpham",
                table: "GioHang",
                column: "idsanpham");

            migrationBuilder.CreateIndex(
                name: "UQ__KhuyenMa__77F420CC6335069A",
                table: "KhuyenMai",
                column: "makhuyenmai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__AB6E6164ACB2545F",
                table: "NguoiDung",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungPhanQuyen_idnguoidung",
                table: "NguoiDungPhanQuyen",
                column: "idnguoidung");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungPhanQuyen_idphanquyen",
                table: "NguoiDungPhanQuyen",
                column: "idphanquyen");

            migrationBuilder.CreateIndex(
                name: "UQ__PhanQuye__FB74F8710F305027",
                table: "PhanQuyen",
                column: "tenPQ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_iddanhmuccon",
                table: "SanPham",
                column: "iddanhmuccon");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_idhang",
                table: "SanPham",
                column: "idhang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "DonHangKhuyenMai");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "NguoiDungPhanQuyen");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "PhanQuyen");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.DropTable(
                name: "DiaChiGiaoHang");

            migrationBuilder.DropTable(
                name: "DanhMucCon");

            migrationBuilder.DropTable(
                name: "Hang");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "DanhMuc");
        }
    }
}
