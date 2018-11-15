using System.Reflection;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Obs;

using Obs.MultiTenancy;
using Obs.Roles;
using Obs.Users;
using LesoftWuye2.Core.Authorization;
using LesoftWuye2.Core.Configuration;

namespace LesoftWuye2.Core
{
    [DependsOn(typeof(ObsCoreModule))]
    public class LesoftWuye2CoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            //Remove the following line to disable multi-tenancy.
            Configuration.MultiTenancy.IsEnabled = true;

            //Add/remove localization sources here
            //Configuration.Localization.Sources.Add(
            //    new DictionaryBasedLocalizationSource(
            //        LesoftWuye2Consts.LocalizationSourceName,
            //        new XmlEmbeddedFileLocalizationDictionaryProvider(
            //            Assembly.GetExecutingAssembly(),
            //            "Obs.Localization.Source"
            //            )
            //        )
            //    );

            //AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<LesoftWuye2AuthorizationProvider>();
            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
