using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Rentsaleinfos;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
using LesoftWuye2.Application.RentsaleinfoTypes;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class RentsaleinfosController : LesoftWuye2ControllerBase
    {
        private readonly IRentsaleinfoAppService _rentsaleinfoAppService;
        private readonly IRentsaleinfoTypeAppService _estateinfoTypeAppService;


        public RentsaleinfosController(IRentsaleinfoAppService rentsaleinfoAppService, IRentsaleinfoTypeAppService estateinfoTypeAppService)
        {
            _rentsaleinfoAppService = rentsaleinfoAppService;
            _estateinfoTypeAppService = estateinfoTypeAppService;
        }

        public async Task<ActionResult> Index()
        {
            ViewData["types"] = await _estateinfoTypeAppService.GetRentsaleinfoTypesForCombo();
            var output = await _rentsaleinfoAppService.GetRentsaleinfos(BuildPageListRequstDto());
            return View(output);
        }
    }
}
