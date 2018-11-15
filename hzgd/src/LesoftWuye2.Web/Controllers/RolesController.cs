using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

using Obs.Dto;
using Obs.Roles;

using PermissionNames = Obs.Authorization.PermissionNames;

namespace LesoftWuye2.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Roles)]
    public class RolesController : LesoftWuye2ControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _roleAppService.GetRoles(BuildPageListRequstDto());
            return View(output);
        }
    }
}












