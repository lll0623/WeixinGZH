using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Plates;
using LesoftWuye2.Application.Categories.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class PlatesController : LesoftWuye2ControllerBase
    { 
        private readonly IPlateAppService _categoryAppService;
        public PlatesController(IPlateAppService categoryAppService) { _categoryAppService = categoryAppService; }
        public async Task<ActionResult> Index() { var output = await _categoryAppService.GetPlates(BuildPageListRequstDto()); return View(output); }
    }
}
