using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.ActivityPersons;
using LesoftWuye2.Application.ActivityPersons.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class ActivityPersonsController :LesoftWuye2ControllerBase 
{
private readonly IActivityPersonAppService _activitypersonAppService;
public ActivityPersonsController(IActivityPersonAppService activitypersonAppService){_activitypersonAppService = activitypersonAppService;}
public async Task<ActionResult> Index(){var output = await _activitypersonAppService.GetActivityPersons(BuildPageListRequstDto());return View(output);}
}
}
