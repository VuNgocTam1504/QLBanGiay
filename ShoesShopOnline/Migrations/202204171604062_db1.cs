namespace ShoesShopOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contact", "Subject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "Subject", c => c.String(maxLength: 200));
        }
    }
}
