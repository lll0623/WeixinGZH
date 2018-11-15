using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.WeixinSubscribes;
using LesoftWuye2.Application.WeixinSubscribes.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers
{
    public class WeixinSubscribesController : LesoftWuye2ControllerBase
    {
        private readonly IWeixinSubscribeAppService _WeixinSubscribeAppService;

        public WeixinSubscribesController(IWeixinSubscribeAppService WeixinSubscribeAppService)
        {
            _WeixinSubscribeAppService = WeixinSubscribeAppService;
        }
        public async Task<ActionResult> Index() { var output = await _WeixinSubscribeAppService.GetWeixinSubscribes(BuildPageListRequstDto()); return View(output); }
    }
}
