namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TemplateKeys",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TempMsgKey = c.String(nullable: false, maxLength: 128),
                        TempMsgId = c.String(maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TemplateKeys");
        }
    }
}
