using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;

using LesoftWuye2.Core;
using LesoftWuye2.EntityFramework.EntityFramework;

namespace LesoftWuye2.EntityFramework
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(LesoftWuye2CoreModule))]
    public class LesoftWuye2DataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LesoftWuye2DbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
