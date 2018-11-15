using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.LifeInfos;
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Application.LifeInfoTypes;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class LifeInfosController : LesoftWuye2ControllerBase
    {
        private readonly ILifeInfoAppService _lifeinfoAppService;
        private readonly ILifeInfoTypeAppService _lifeinfotypeAppService;
        private readonly IProjectAppService _projectAppService;

        public LifeInfosController(
            ILifeInfoAppService lifeinfoAppService,
            ILifeInfoTypeAppService lifeinfotypeAppService, 
            IProjectAppService projectAppService)
        {
            _lifeinfoAppService = lifeinfoAppService;
            _lifeinfotypeAppService = lifeinfotypeAppService;
            _projectAppService = projectAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _lifeinfoAppService.GetLifeInfos(BuildPageListRequstDto());
            ViewData["types"] = await _lifeinfotypeAppService.GetLifeInfoTypesForCombo();
            ViewData["projects"] = await _projectAppService.GetProjectsForCombo();

            return View(output);
        }
    }
}
