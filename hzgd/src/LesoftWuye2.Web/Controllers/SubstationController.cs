using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Substations;
using LesoftWuye2.Application.Substations.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class SubstationsController : LesoftWuye2ControllerBase
    {
        private readonly ISubstationAppService _substationAppService;
        public SubstationsController(ISubstationAppService substationAppService) { _substationAppService = substationAppService; }
        public async Task<ActionResult> Index() { var output = await _substationAppService.GetSubstations(BuildPageListRequstDto()); return View(output); }
    }
}
