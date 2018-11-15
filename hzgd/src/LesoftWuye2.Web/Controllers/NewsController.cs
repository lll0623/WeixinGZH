using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Newss;
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class NewssController : LesoftWuye2ControllerBase
    {
        private readonly INewsAppService _newsAppService;
        private readonly IProjectAppService _projectAppService;

        public NewssController(INewsAppService newsAppService, IProjectAppService projectAppService)
        {
            _newsAppService = newsAppService;
            _projectAppService = projectAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _newsAppService.GetNewss(BuildPageListRequstDto());
            ViewData["projects"] = await _projectAppService.GetProjectsForCombo();
            return View(output);
        }
    }
}
