using System.Reflection;
using Abp.Modules;
using LesoftWuye2.Core;

namespace LesoftWuye2.CodeGenerator
{
    [DependsOn(typeof(LesoftWuye2CoreModule))]
    public class LesoftWuye2CodeGeneratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
