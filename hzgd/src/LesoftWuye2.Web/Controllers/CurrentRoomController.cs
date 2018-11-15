using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using LesoftWuye2.Application.CurrentRooms;
using LesoftWuye2.Application.CurrentRooms.Dto;
using PermissionNames = LesoftWuye2.Core.Authorization.PermissionNames;
namespace LesoftWuye2.Web.Controllers 
 {  public class CurrentRoomsController :LesoftWuye2ControllerBase 
{
private readonly ICurrentRoomAppService _currentroomAppService;
public CurrentRoomsController(ICurrentRoomAppService currentroomAppService){_currentroomAppService = currentroomAppService;}
public async Task<ActionResult> Index(){var output = await _currentroomAppService.GetCurrentRooms(BuildPageListRequstDto());return View(output);}
}
}
