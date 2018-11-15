using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.PayNotifys;
using LesoftWuye2.Application.PayNotifys.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class PayNotifysController :LesoftWuye2ControllerBase 
{
private readonly IPayNotifyAppService _paynotifyAppService;
public PayNotifysController(IPayNotifyAppService paynotifyAppService){_paynotifyAppService = paynotifyAppService;}
public async Task<ActionResult> Index(){var output = await _paynotifyAppService.GetPayNotifys(BuildPageListRequstDto());return View(output);}
}
}
