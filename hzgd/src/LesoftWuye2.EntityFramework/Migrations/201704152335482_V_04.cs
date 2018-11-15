namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_04 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ServiceTags", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ServiceTags", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
