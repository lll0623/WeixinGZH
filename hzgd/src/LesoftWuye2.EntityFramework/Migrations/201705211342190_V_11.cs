namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderPays", "RefundsPayTime", c => c.DateTime());
            AddColumn("dbo.OrderPays", "RefundsTradeNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderPays", "RefundsTradeNo");
            DropColumn("dbo.OrderPays", "RefundsPayTime");
        }
    }
}
