namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefundOrderImages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Thumbnail = c.String(maxLength: 128),
                        RefundOrderId = c.Long(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefundOrders", t => t.RefundOrderId)
                .Index(t => t.RefundOrderId);
            
            CreateTable(
                "dbo.RefundOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderId = c.Long(nullable: false),
                        OrderNo = c.String(),
                        MemberId = c.Long(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Specification = c.String(nullable: false, maxLength: 50),
                        Unit = c.String(nullable: false, maxLength: 20),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Thumbnail = c.String(maxLength: 128),
                        Num = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Reason = c.Int(nullable: false),
                        RefundMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Mobile = c.String(),
                        Remark = c.String(maxLength: 200),
                        Status = c.Int(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.MemberId);
            
            AddColumn("dbo.Orders", "RefundStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RefundOrderImages", "RefundOrderId", "dbo.RefundOrders");
            DropForeignKey("dbo.RefundOrders", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.RefundOrders", "MemberId", "dbo.Members");
            DropIndex("dbo.RefundOrders", new[] { "MemberId" });
            DropIndex("dbo.RefundOrders", new[] { "OrderId" });
            DropIndex("dbo.RefundOrderImages", new[] { "RefundOrderId" });
            DropColumn("dbo.Orders", "RefundStatus");
            DropTable("dbo.RefundOrders");
            DropTable("dbo.RefundOrderImages");
        }
    }
}
