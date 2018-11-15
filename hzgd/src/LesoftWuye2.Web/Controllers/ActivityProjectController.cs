using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.ActivityProjects;
using LesoftWuye2.Application.ActivityProjects.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class ActivityProjectsController :LesoftWuye2ControllerBase 
{
private readonly IActivityProjectAppService _activityprojectAppService;
public ActivityProjectsController(IActivityProjectAppService activityprojectAppService){_activityprojectAppService = activityprojectAppService;}
public async Task<ActionResult> Index(){var output = await _activityprojectAppService.GetActivityProjects(BuildPageListRequstDto());return View(output);}
}
}
