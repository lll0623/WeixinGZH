namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_05 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Areas");
            AlterColumn("dbo.Areas", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Areas", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Areas");
            AlterColumn("dbo.Areas", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Areas", "Id");
        }
    }
}
