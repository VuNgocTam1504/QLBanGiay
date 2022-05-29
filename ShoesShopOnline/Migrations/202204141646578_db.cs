namespace ShoesShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnhMoTa",
                c => new
                    {
                        MaAnh = c.Int(nullable: false, identity: true),
                        HinhAnh = c.String(maxLength: 300),
                        MaSP = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.MaAnh)
                .ForeignKey("dbo.SanPham", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.ChiTietSanPham",
                c => new
                    {
                        MaAnh = c.Int(nullable: false),
                        KichCo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MaAnh, t.KichCo })
                .ForeignKey("dbo.AnhMoTa", t => t.MaAnh, cascadeDelete: true)
                .Index(t => t.MaAnh);
            
            CreateTable(
                "dbo.ChiTietHoaDon",
                c => new
                    {
                        MaHD = c.Int(nullable: false),
                        MaAnh = c.Int(nullable: false),
                        KichCo = c.Int(nullable: false),
                        SoluongMua = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MaHD, t.MaAnh, t.KichCo })
                .ForeignKey("dbo.HoaDon", t => t.MaHD, cascadeDelete: true)
                .ForeignKey("dbo.ChiTietSanPham", t => new { t.MaAnh, t.KichCo }, cascadeDelete: true)
                .Index(t => t.MaHD)
                .Index(t => new { t.MaAnh, t.KichCo });
            
            CreateTable(
                "dbo.HoaDon",
                c => new
                    {
                        MaHD = c.Int(nullable: false, identity: true),
                        MaTK = c.Int(),
                        HoTenNguoiNhan = c.String(nullable: false, maxLength: 50),
                        SDTNguoiNhan = c.String(nullable: false, maxLength: 20, unicode: false),
                        DiaChiNhan = c.String(nullable: false, maxLength: 100),
                        EmailNguoiNhan = c.String(maxLength: 50, unicode: false),
                        NgayLap = c.DateTime(nullable: false),
                        TrangThai = c.String(maxLength: 50),
                        GhiChu = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaHD)
                .ForeignKey("dbo.TaiKhoanNguoiDung", t => t.MaTK, cascadeDelete: true)
                .Index(t => t.MaTK);
            
            CreateTable(
                "dbo.TaiKhoanNguoiDung",
                c => new
                    {
                        MaTK = c.Int(nullable: false, identity: true),
                        TenDangNhap = c.String(nullable: false, maxLength: 100),
                        MatKhau = c.String(nullable: false, maxLength: 20),
                        HoTen = c.String(nullable: false, maxLength: 200),
                        SDT = c.String(nullable: false, maxLength: 20, fixedLength: true, unicode: false),
                        DiaChi = c.String(nullable: false, maxLength: 500),
                        Email = c.String(maxLength: 50, unicode: false),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaTK);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        MaSP = c.String(nullable: false, maxLength: 10, unicode: false),
                        TenSP = c.String(nullable: false, maxLength: 200),
                        MaDM = c.String(nullable: false, maxLength: 20, unicode: false),
                        MoTa = c.String(nullable: false, maxLength: 500),
                        GiaBan = c.Decimal(nullable: false, storeType: "money"),
                        NgayTao = c.DateTime(nullable: false),
                        NgaySua = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.DanhMucSP", t => t.MaDM, cascadeDelete: true)
                .Index(t => t.MaDM);
            
            CreateTable(
                "dbo.DanhMucSP",
                c => new
                    {
                        MaDM = c.String(nullable: false, maxLength: 20, unicode: false),
                        TenDanhMuc = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.MaDM);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(maxLength: 100),
                        Subject = c.String(maxLength: 200),
                        Content = c.String(maxLength: 500),
                        Email = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 30, unicode: false),
                        DateContact = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.TaiKhoanQuanTri",
                c => new
                    {
                        MaTK = c.Int(nullable: false, identity: true),
                        TenDangNhap = c.String(nullable: false, maxLength: 50, unicode: false),
                        MatKhau = c.String(nullable: false, maxLength: 50, unicode: false),
                        HoTenUser = c.String(nullable: false, maxLength: 50),
                        LoaiTK = c.String(nullable: false, maxLength: 50),
                        TrangThai = c.Boolean(nullable: false),
                        SDT = c.String(maxLength: 20, fixedLength: true, unicode: false),
                        DiaChi = c.String(maxLength: 500),
                        Email = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.MaTK);
            
            CreateTable(
                "dbo.TinTuc",
                c => new
                    {
                        MaTin = c.Int(nullable: false, identity: true),
                        TenTin = c.String(nullable: false, maxLength: 100),
                        NgayDang = c.DateTime(nullable: false),
                        MaTK = c.Int(),
                        NoiDung = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.MaTin)
                .ForeignKey("dbo.TaiKhoanQuanTri", t => t.MaTK, cascadeDelete: true)
                .Index(t => t.MaTK);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TinTuc", "MaTK", "dbo.TaiKhoanQuanTri");
            DropForeignKey("dbo.SanPham", "MaDM", "dbo.DanhMucSP");
            DropForeignKey("dbo.AnhMoTa", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietHoaDon", new[] { "MaAnh", "KichCo" }, "dbo.ChiTietSanPham");
            DropForeignKey("dbo.HoaDon", "MaTK", "dbo.TaiKhoanNguoiDung");
            DropForeignKey("dbo.ChiTietHoaDon", "MaHD", "dbo.HoaDon");
            DropForeignKey("dbo.ChiTietSanPham", "MaAnh", "dbo.AnhMoTa");
            DropIndex("dbo.TinTuc", new[] { "MaTK" });
            DropIndex("dbo.SanPham", new[] { "MaDM" });
            DropIndex("dbo.HoaDon", new[] { "MaTK" });
            DropIndex("dbo.ChiTietHoaDon", new[] { "MaAnh", "KichCo" });
            DropIndex("dbo.ChiTietHoaDon", new[] { "MaHD" });
            DropIndex("dbo.ChiTietSanPham", new[] { "MaAnh" });
            DropIndex("dbo.AnhMoTa", new[] { "MaSP" });
            DropTable("dbo.TinTuc");
            DropTable("dbo.TaiKhoanQuanTri");
            DropTable("dbo.Contact");
            DropTable("dbo.DanhMucSP");
            DropTable("dbo.SanPham");
            DropTable("dbo.TaiKhoanNguoiDung");
            DropTable("dbo.HoaDon");
            DropTable("dbo.ChiTietHoaDon");
            DropTable("dbo.ChiTietSanPham");
            DropTable("dbo.AnhMoTa");
        }
    }
}
