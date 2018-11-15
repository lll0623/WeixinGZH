using EntityFramework.DynamicFilters;
using LesoftWuye2.EntityFramework.EntityFramework;

namespace LesoftWuye2.EntityFramework.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly LesoftWuye2DbContext _context;

        public InitialHostDbBuilder(LesoftWuye2DbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new LesoftWuye2.EntityFramework.Migrations.SeedData.DefaultEditionsCreator(_context).Create();
            new LesoftWuye2.EntityFramework.Migrations.SeedData.DefaultLanguagesCreator(_context).Create();
            new LesoftWuye2.EntityFramework.Migrations.SeedData.HostRoleAndUserCreator(_context).Create();
            new LesoftWuye2.EntityFramework.Migrations.SeedData.DefaultSettingsCreator(_context).Create();
        }
    }
}
