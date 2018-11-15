using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Categorys;
using LesoftWuye2.Application.Products;
using LesoftWuye2.Application.Products.Dto;
using LesoftWuye2.Application.ProductSlideImages;
using LesoftWuye2.Application.Suppliers;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class ProductCommentsController : LesoftWuye2ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly ISupplierAppService _supplierAppService;
        private readonly IProductSlideImageAppService _productSlideImageAppService;

        public ProductCommentsController(IProductAppService productAppService,
            ICategoryAppService categoryAppService,
            ISupplierAppService supplierAppService,
            IProductSlideImageAppService productSlideImageAppService)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
            _supplierAppService = supplierAppService;
            _productSlideImageAppService = productSlideImageAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _productAppService.GetProductComments(BuildPageListRequstDto());
            return View(output);
        }
    }
}
