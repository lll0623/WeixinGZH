using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.NewsProjects;
using LesoftWuye2.Application.NewsProjects.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class NewsProjectsController :LesoftWuye2ControllerBase 
{
private readonly INewsProjectAppService _newsprojectAppService;
public NewsProjectsController(INewsProjectAppService newsprojectAppService){_newsprojectAppService = newsprojectAppService;}
public async Task<ActionResult> Index(){var output = await _newsprojectAppService.GetNewsProjects(BuildPageListRequstDto());return View(output);}
}
}
