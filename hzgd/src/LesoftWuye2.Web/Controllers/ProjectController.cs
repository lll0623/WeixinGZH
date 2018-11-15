using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Projects;
using LesoftWuye2.Application.Projects.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class ProjectsController :LesoftWuye2ControllerBase 
{
private readonly IProjectAppService _projectAppService;
public ProjectsController(IProjectAppService projectAppService){_projectAppService = projectAppService;}
public async Task<ActionResult> Index(){var output = await _projectAppService.GetProjects(BuildPageListRequstDto());return View(output);}
}
}
