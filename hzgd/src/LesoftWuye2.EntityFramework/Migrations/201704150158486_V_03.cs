namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Openid", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Openid", c => c.String());
        }
    }
}
