using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.MallSlideImages;
using LesoftWuye2.Application.MallSlideImages.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class MallSlideImagesController : LesoftWuye2ControllerBase
    {
        private readonly IMallSlideImageAppService _mallslideimageAppService;
        public MallSlideImagesController(IMallSlideImageAppService mallslideimageAppService) { _mallslideimageAppService = mallslideimageAppService; }
        public async Task<ActionResult> Index() { var output = await _mallslideimageAppService.GetMallSlideImages(BuildPageListRequstDto()); return View(output); }
    }
}
