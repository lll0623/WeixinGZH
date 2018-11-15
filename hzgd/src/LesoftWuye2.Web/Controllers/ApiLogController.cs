using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LesoftWuye2.Application.ApiLogs;

namespace LesoftWuye2.Web.Controllers
{
    public class ApiLogsController : LesoftWuye2ControllerBase
    {
        private readonly IApiLogAppService _apilogAppService;

        public ApiLogsController(IApiLogAppService apilogAppService)
        {
            _apilogAppService = apilogAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _apilogAppService.GetApiLogs(BuildPageListRequstDto());
            return View(output);
        }
    }
}
