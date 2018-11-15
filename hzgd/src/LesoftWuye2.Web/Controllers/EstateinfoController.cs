using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Estateinfos;
using LesoftWuye2.Application.Estateinfos.Dto;
using LesoftWuye2.Application.EstateinfoTypes;
using LesoftWuye2.Application.Projects;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class EstateinfosController : LesoftWuye2ControllerBase
    {
        private readonly IEstateinfoAppService _newsAppService;
        private readonly IEstateinfoTypeAppService _estateinfoTypeAppService;


        public EstateinfosController(IEstateinfoAppService newsAppService, IEstateinfoTypeAppService estateinfoTypeAppService)
        {
            _newsAppService = newsAppService;
            _estateinfoTypeAppService = estateinfoTypeAppService;
        }

        public async Task<ActionResult> Index()
        {
            ViewData["types"] = await _estateinfoTypeAppService.GetEstateinfoTypesForCombo();
            var output = await _newsAppService.GetEstateinfos(BuildPageListRequstDto());
            return View(output);
        }
    }
}
