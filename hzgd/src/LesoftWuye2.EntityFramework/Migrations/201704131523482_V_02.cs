namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "ProjectCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "ProjectCode");
        }
    }
}
