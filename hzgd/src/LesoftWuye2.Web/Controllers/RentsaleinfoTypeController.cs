using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.RentsaleinfoTypes;
using LesoftWuye2.Application.RentsaleinfoTypes.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class RentsaleinfoTypesController : LesoftWuye2ControllerBase
    {
        private readonly IRentsaleinfoTypeAppService _rentsaleinfotypeAppService;

        public RentsaleinfoTypesController(IRentsaleinfoTypeAppService rentsaleinfotypeAppService)
        {
            _rentsaleinfotypeAppService = rentsaleinfotypeAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _rentsaleinfotypeAppService.GetRentsaleinfoTypes(BuildPageListRequstDto());
            return View(output);
        }
    }
}
