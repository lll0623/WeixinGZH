using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Localization;
using Abp.Zero.Configuration;
using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Hangfire;
using Obs;
using LesoftWuye2.EntityFramework;
using LesoftWuye2.Application;
using LesoftWuye2.Application.Jobs;
using LesoftWuye2.WebApi.Api;

namespace LesoftWuye2.Web
{
    [DependsOn(
        typeof(LesoftWuye2DataModule),
        typeof(LesoftWuye2ApplicationModule),
        typeof(LesoftWuye2WebApiModule),
        typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule))]
    public class LesoftWuye2WebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //IocManager.Register<ILocalizationManager, NullLocalizationManager>();

            //Enable database based localization
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<LesoftWuye2NavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;

           
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");
            appFolders.WebLogsFolder = server.MapPath("~/App_Data/Logs");
            appFolders.UploadImageFolder = server.MapPath("~/Upload/Images");
            appFolders.Root = server.MapPath("~");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
            try { DirectoryHelper.CreateIfNotExists(appFolders.UploadImageFolder); } catch { }


            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<GrouponCancelWorker>());
            workManager.Add(IocManager.Resolve<GrouponRefundsWorker>());
            workManager.Add(IocManager.Resolve<OrderRefundWorker>());
        }
    }
}













