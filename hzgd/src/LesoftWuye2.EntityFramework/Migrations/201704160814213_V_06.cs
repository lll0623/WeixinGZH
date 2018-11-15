namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_06 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductLikes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        MemberId = c.Long(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductLikes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductLikes", "MemberId", "dbo.Members");
            DropIndex("dbo.ProductLikes", new[] { "MemberId" });
            DropIndex("dbo.ProductLikes", new[] { "ProductId" });
            DropTable("dbo.ProductLikes");
        }
    }
}
