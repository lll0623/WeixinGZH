using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Activitys;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class ActivitysController : LesoftWuye2ControllerBase
    {
        private readonly IActivityAppService _activityAppService;
        private readonly IProjectAppService _projectAppService;

        public ActivitysController(IActivityAppService activityAppService, IProjectAppService projectAppService)
        {
            _activityAppService = activityAppService;
            _projectAppService = projectAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _activityAppService.GetActivitys(BuildPageListRequstDto());
            ViewData["projects"] = await _projectAppService.GetProjectsForCombo();
            return View(output);
        }

        public async Task<ActionResult> Person(long id)
        {
            var output = await _activityAppService.GetActivityPersons(id,BuildPageListRequstDto());
            return View(output);
        }
    }
}
