using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.Members;
using LesoftWuye2.Application.Members.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class MembersController :LesoftWuye2ControllerBase 
{
private readonly IMemberAppService _memberAppService;
public MembersController(IMemberAppService memberAppService){_memberAppService = memberAppService;}
public async Task<ActionResult> Index(){var output = await _memberAppService.GetMembers(BuildPageListRequstDto());return View(output);}
}
}
