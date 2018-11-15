using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.EstateinfoComments;
using LesoftWuye2.Application.EstateinfoComments.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class EstateinfoCommentsController :LesoftWuye2ControllerBase 
{
private readonly IEstateinfoCommentAppService _estateinfocommentAppService;
public EstateinfoCommentsController(IEstateinfoCommentAppService estateinfocommentAppService){_estateinfocommentAppService = estateinfocommentAppService;}
public async Task<ActionResult> Index(){var output = await _estateinfocommentAppService.GetEstateinfoComments(BuildPageListRequstDto());return View(output);}
}
}
