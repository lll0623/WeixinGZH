using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Groupons;
using LesoftWuye2.Application.Groupons.Dto;
using LesoftWuye2.Application.Products;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class GrouponsController : LesoftWuye2ControllerBase
    {
        private readonly IGrouponAppService _grouponAppService;
        private readonly IProductAppService _productAppService;

        public GrouponsController(IGrouponAppService grouponAppService,
            IProductAppService productAppService)
        {
            _grouponAppService = grouponAppService;
            _productAppService = productAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _grouponAppService.GetGroupons(BuildPageListRequstDto());
            ViewData["products"] = await _productAppService.GetProductsForCombo();
            ViewData["host"] =this.Request.Url.Scheme+"://"+  this.Request.Url.Host;
            return View(output);
        }
    }
}
