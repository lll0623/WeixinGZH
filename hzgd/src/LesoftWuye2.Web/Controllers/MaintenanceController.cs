using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Obs.Auditing;
using Obs.Caching;
using Obs.Logging;
using Obs.Users;

using PermissionNames = Obs.Authorization.PermissionNames;

namespace LesoftWuye2.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Maintenance)]
    public class MaintenanceController : LesoftWuye2ControllerBase
    {
        private readonly ICachingAppService _cachingAppService;
        private readonly IWebLogAppService _webLogAppService;

        public MaintenanceController(ICachingAppService cachingAppService, IWebLogAppService webLogAppService)
        {
            _cachingAppService = cachingAppService;
            _webLogAppService = webLogAppService;
        }

        public  ActionResult Index()
        {
            var output = _cachingAppService.GetAllCaches();
            var logLines= _webLogAppService.GetLatestWebLogs();
            ViewData["logs"] = logLines.LatesWebLogLines;
            return View(output);
        }
    }
}












