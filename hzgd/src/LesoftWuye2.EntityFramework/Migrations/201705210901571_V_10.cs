namespace LesoftWuye2.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V_10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeeServiceProjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FeeServiceId = c.Long(nullable: false),
                        ProjectId = c.Long(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeeServices", t => t.FeeServiceId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.FeeServiceId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.FeeServices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        AllProjects = c.Boolean(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeeServiceProjects", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.FeeServiceProjects", "FeeServiceId", "dbo.FeeServices");
            DropIndex("dbo.FeeServiceProjects", new[] { "ProjectId" });
            DropIndex("dbo.FeeServiceProjects", new[] { "FeeServiceId" });
            DropTable("dbo.FeeServices");
            DropTable("dbo.FeeServiceProjects");
        }
    }
}
