namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groupons", "StartTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groupons", "StartTime");
        }
    }
}
