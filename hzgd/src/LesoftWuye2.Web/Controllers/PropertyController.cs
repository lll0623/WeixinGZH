using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.PropertyCitys;
using LesoftWuye2.Application.Propertys;
using LesoftWuye2.Application.Propertys.Dto;
using LesoftWuye2.Application.PropertyTypes;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class PropertysController : LesoftWuye2ControllerBase
    {
        private readonly IPropertyAppService _propertyAppService;
        private readonly IPropertyCityAppService _propertyCityAppService;
        private readonly IPropertyTypeAppService _propertyTypeAppService;

        public PropertysController(IPropertyAppService propertyAppService,
            IPropertyCityAppService propertyCityAppService,
            IPropertyTypeAppService propertyTypeAppService
            )
        {
            _propertyAppService = propertyAppService;
            _propertyCityAppService = propertyCityAppService;
            _propertyTypeAppService = propertyTypeAppService;

        }

        public async Task<ActionResult> Index()
        {
            var output = await _propertyAppService.GetPropertys(BuildPageListRequstDto());
            ViewData["types"] = await _propertyTypeAppService.GetPropertyTypesForCombo();
            ViewData["citys"] = await _propertyCityAppService.GetPropertyCitysForCombo();
            return View(output);
        }
    }
}
