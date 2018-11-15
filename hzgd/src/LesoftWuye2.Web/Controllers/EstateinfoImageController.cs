using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.EstateinfoImages;
using LesoftWuye2.Application.EstateinfoImages.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class EstateinfoImagesController :LesoftWuye2ControllerBase 
{
private readonly IEstateinfoImageAppService _estateinfoimageAppService;
public EstateinfoImagesController(IEstateinfoImageAppService estateinfoimageAppService){_estateinfoimageAppService = estateinfoimageAppService;}
public async Task<ActionResult> Index(){var output = await _estateinfoimageAppService.GetEstateinfoImages(BuildPageListRequstDto());return View(output);}
}
}
