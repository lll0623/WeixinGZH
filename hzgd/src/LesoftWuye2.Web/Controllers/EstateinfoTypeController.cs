using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.EstateinfoTypes;
using LesoftWuye2.Application.EstateinfoTypes.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class EstateinfoTypesController : LesoftWuye2ControllerBase
    {
        private readonly IEstateinfoTypeAppService _lifeinfotypeAppService;
        public EstateinfoTypesController(IEstateinfoTypeAppService lifeinfotypeAppService) { _lifeinfotypeAppService = lifeinfotypeAppService; }
        public async Task<ActionResult> Index() { var output = await _lifeinfotypeAppService.GetEstateinfoTypes(BuildPageListRequstDto()); return View(output); }
    }
}
