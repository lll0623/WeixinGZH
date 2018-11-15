using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.LifeInfoProjects;
using LesoftWuye2.Application.LifeInfoProjects.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class LifeInfoProjectsController :LesoftWuye2ControllerBase 
{
private readonly ILifeInfoProjectAppService _lifeinfoprojectAppService;
public LifeInfoProjectsController(ILifeInfoProjectAppService lifeinfoprojectAppService){_lifeinfoprojectAppService = lifeinfoprojectAppService;}
public async Task<ActionResult> Index(){var output = await _lifeinfoprojectAppService.GetLifeInfoProjects(BuildPageListRequstDto());return View(output);}
}
}
