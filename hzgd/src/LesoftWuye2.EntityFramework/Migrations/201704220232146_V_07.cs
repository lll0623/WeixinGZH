namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_07 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeixinSubscribes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Summary = c.String(nullable: false, maxLength: 500),
                        Thumbnail = c.String(nullable: false, maxLength: 128),
                        Url = c.String(nullable: false, maxLength: 128),
                        Sort = c.Int(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeixinSubscribes");
        }
    }
}
