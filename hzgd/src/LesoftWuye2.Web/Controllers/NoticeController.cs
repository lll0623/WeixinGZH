using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Notices;
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class NoticesController : LesoftWuye2ControllerBase
    {
        private readonly INoticeAppService _noticeAppService;
        private readonly IProjectAppService _projectAppService;

        public NoticesController(INoticeAppService noticeAppService, IProjectAppService projectAppService)
        {
            _noticeAppService = noticeAppService;
            _projectAppService = projectAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _noticeAppService.GetNotices(BuildPageListRequstDto());
            ViewData["projects"] = await _projectAppService.GetProjectsForCombo();
            return View(output);
        }
    }
}
