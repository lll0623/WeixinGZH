namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ClientRemark", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ClientRemark");
        }
    }
}
