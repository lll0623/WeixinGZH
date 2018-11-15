using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.ServiceTels;
using LesoftWuye2.Application.ServiceTels.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class ServiceTelsController :LesoftWuye2ControllerBase 
{
private readonly IServiceTelAppService _servicetelAppService;
public ServiceTelsController(IServiceTelAppService servicetelAppService){_servicetelAppService = servicetelAppService;}
public async Task<ActionResult> Index(){var output = await _servicetelAppService.GetServiceTels(BuildPageListRequstDto());return View(output);}
}
}
