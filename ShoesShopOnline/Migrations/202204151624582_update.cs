namespace ShoesShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KhuyenMaiSP",
                c => new
                    {
                        MaKM = c.Int(nullable: false, identity: true),
                        PhanTramKM = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NgayBatDau = c.DateTime(nullable: false),
                        NgayKetThuc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MaKM);
            
            AddColumn("dbo.SanPham", "MaKM", c => c.Int(nullable: false));
            CreateIndex("dbo.SanPham", "MaKM");
            AddForeignKey("dbo.SanPham", "MaKM", "dbo.KhuyenMaiSP", "MaKM", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", "MaKM", "dbo.KhuyenMaiSP");
            DropIndex("dbo.SanPham", new[] { "MaKM" });
            DropColumn("dbo.SanPham", "MaKM");
            DropTable("dbo.KhuyenMaiSP");
        }
    }
}
