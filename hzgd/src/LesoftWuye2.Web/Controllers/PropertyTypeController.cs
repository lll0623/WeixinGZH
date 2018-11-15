using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.PropertyTypes;
using LesoftWuye2.Application.PropertyTypes.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class PropertyTypesController :LesoftWuye2ControllerBase 
{
private readonly IPropertyTypeAppService _propertytypeAppService;
public PropertyTypesController(IPropertyTypeAppService propertytypeAppService){_propertytypeAppService = propertytypeAppService;}
public async Task<ActionResult> Index(){var output = await _propertytypeAppService.GetPropertyTypes(BuildPageListRequstDto());return View(output);}
}
}
