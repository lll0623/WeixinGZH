using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.PropertyCitys;
using LesoftWuye2.Application.PropertyCitys.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class PropertyCitysController :LesoftWuye2ControllerBase 
{
private readonly IPropertyCityAppService _propertycityAppService;
public PropertyCitysController(IPropertyCityAppService propertycityAppService){_propertycityAppService = propertycityAppService;}
public async Task<ActionResult> Index(){var output = await _propertycityAppService.GetPropertyCitys(BuildPageListRequstDto());return View(output);}
}
}
