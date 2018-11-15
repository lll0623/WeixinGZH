using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.RentsaleinfoImages;
using LesoftWuye2.Application.RentsaleinfoImages.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class RentsaleinfoImagesController :LesoftWuye2ControllerBase 
{
private readonly IRentsaleinfoImageAppService _rentsaleinfoimageAppService;
public RentsaleinfoImagesController(IRentsaleinfoImageAppService rentsaleinfoimageAppService){_rentsaleinfoimageAppService = rentsaleinfoimageAppService;}
public async Task<ActionResult> Index(){var output = await _rentsaleinfoimageAppService.GetRentsaleinfoImages(BuildPageListRequstDto());return View(output);}
}
}
