namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
