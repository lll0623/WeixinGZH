using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.FeeServices;
using LesoftWuye2.Application.FeeServices.Dto;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class FeeServicesController : LesoftWuye2ControllerBase
    {
        private readonly IFeeServiceAppService _feeServiceAppService;
        private readonly IProjectAppService _projectAppService;

        public FeeServicesController(IFeeServiceAppService feeServiceAppService, IProjectAppService projectAppService)
        {
            _feeServiceAppService = feeServiceAppService;
            _projectAppService = projectAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _feeServiceAppService.GetFeeServices(BuildPageListRequstDto());
            ViewData["projects"] = await _projectAppService.GetProjectsForCombo();
            return View(output);
        }
    }
}
