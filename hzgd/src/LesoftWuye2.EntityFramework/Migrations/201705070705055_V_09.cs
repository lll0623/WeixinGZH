namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_09 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Thumbnail", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Thumbnail", c => c.String(maxLength: 128));
        }
    }
}
