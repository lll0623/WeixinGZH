using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.SlideImages;
using LesoftWuye2.Application.SlideImages.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class SlideImagesController :LesoftWuye2ControllerBase 
{
private readonly ISlideImageAppService _slideimageAppService;
public SlideImagesController(ISlideImageAppService slideimageAppService){_slideimageAppService = slideimageAppService;}
public async Task<ActionResult> Index(){var output = await _slideimageAppService.GetSlideImages(BuildPageListRequstDto());return View(output);}
}
}
