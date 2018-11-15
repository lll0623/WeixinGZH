using System.Linq;
using Obs.MultiTenancy;
using LesoftWuye2.EntityFramework.EntityFramework;

namespace LesoftWuye2.EntityFramework.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly LesoftWuye2DbContext _context;

        public DefaultTenantCreator(LesoftWuye2DbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
