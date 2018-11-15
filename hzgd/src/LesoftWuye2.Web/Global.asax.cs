using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using Exceptionless;

namespace LesoftWuye2.Web
{
    public class MvcApplication : AbpWebApplication<LesoftWuye2WebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            ExceptionlessClient.Default.Startup("Gs3zPjFxAo5Q7Rw7fWhEdhSmLb0qNqlnr1VVbBSM");
            ExceptionlessClient.Default.Configuration.ServerUrl = "http://121.40.117.174:8004/";

            base.Application_Start(sender, e);
        }
    }
}













