using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.CurrentRooms.Dto;
namespace LesoftWuye2.Application.CurrentRooms {  public interface ICurrentRoomAppService : IApplicationService 
{
Task CreateCurrentRoom(CreateCurrentRoomInput input);
Task DeleteCurrentRoom(long id);
Task UpdateCurrentRoom(UpdateCurrentRoomInput input);
Task<PageListResultDto<CurrentRoomListDto>> GetCurrentRooms(GetPageListRequstDto dto);
Task<CurrentRoomItemDto> GetCurrentRoom(long id);
}
}
