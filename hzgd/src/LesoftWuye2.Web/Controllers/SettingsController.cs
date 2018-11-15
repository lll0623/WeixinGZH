using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Configuration;
using LesoftWuye2.Core.Authorization;

namespace LesoftWuye2.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Settings)]
    public class SettingsController : LesoftWuye2ControllerBase
    {
        private readonly ISettingsAppService _settingsAppService;


        public SettingsController(ISettingsAppService settingsAppService)
        {
            _settingsAppService = settingsAppService;

        }

        public async Task<ActionResult> Index()
        {
            var output = await _settingsAppService.GetAllSettings();
            return View(output);
        }
    }
}












