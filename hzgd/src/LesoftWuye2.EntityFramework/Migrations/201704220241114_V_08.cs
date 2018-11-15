namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_08 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WeixinSubscribes", "Url", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WeixinSubscribes", "Url", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
