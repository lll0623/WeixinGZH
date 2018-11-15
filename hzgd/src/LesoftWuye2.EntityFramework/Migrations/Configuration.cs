using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using EntityFramework.DynamicFilters;
using LesoftWuye2.EntityFramework.EntityFramework;
using LesoftWuye2.EntityFramework.Migrations.SeedData;
using DefaultTenantCreator = LesoftWuye2.EntityFramework.Migrations.SeedData.DefaultTenantCreator;
using InitialHostDbBuilder = LesoftWuye2.EntityFramework.Migrations.SeedData.InitialHostDbBuilder;
using TenantRoleAndUserBuilder = LesoftWuye2.EntityFramework.Migrations.SeedData.TenantRoleAndUserBuilder;

namespace LesoftWuye2.EntityFramework.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<LesoftWuye2DbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LesoftWuye2";
        }

        protected override void Seed(LesoftWuye2DbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

              
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
