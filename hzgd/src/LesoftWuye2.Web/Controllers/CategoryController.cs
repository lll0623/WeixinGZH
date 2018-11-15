using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Categorys;
using LesoftWuye2.Application.Categories.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class CategorysController : LesoftWuye2ControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;
        public CategorysController(ICategoryAppService categoryAppService) { _categoryAppService = categoryAppService; }
        public async Task<ActionResult> Index() { var output = await _categoryAppService.GetCategorys(BuildPageListRequstDto()); return View(output); }
    }
}
