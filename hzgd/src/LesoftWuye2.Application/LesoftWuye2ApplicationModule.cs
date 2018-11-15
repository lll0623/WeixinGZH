using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using LesoftWuye2.Core;
using Obs;

namespace LesoftWuye2.Application
{
    [DependsOn(typeof(LesoftWuye2CoreModule), typeof(ObsApplicationModule), typeof(AbpAutoMapperModule))]
    public class LesoftWuye2ApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


        }
    }
}
