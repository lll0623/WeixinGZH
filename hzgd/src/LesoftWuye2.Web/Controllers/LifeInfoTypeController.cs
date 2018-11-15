using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.LifeInfoTypes;
using LesoftWuye2.Application.LifeInfoTypes.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class LifeInfoTypesController :LesoftWuye2ControllerBase 
{
private readonly ILifeInfoTypeAppService _lifeinfotypeAppService;
public LifeInfoTypesController(ILifeInfoTypeAppService lifeinfotypeAppService){_lifeinfotypeAppService = lifeinfotypeAppService;}
public async Task<ActionResult> Index(){var output = await _lifeinfotypeAppService.GetLifeInfoTypes(BuildPageListRequstDto());return View(output);}
}
}
