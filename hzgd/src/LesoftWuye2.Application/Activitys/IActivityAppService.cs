using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Activitys.Dto;
namespace LesoftWuye2.Application.Activitys
{
    public interface IActivityAppService : IApplicationService
    {
        Task CreateActivity(CreateActivityInput input);
        Task DeleteActivity(long id);
        Task UpdateActivity(UpdateActivityInput input);
        Task<PageListResultDto<ActivityListDto>> GetActivitys(GetPageListRequstDto dto);
        Task<ActivityItemDto> GetActivity(long id);

        Task<PageListResultDto<ActivityPersonListDto>> GetActivityPersons(long activityId, GetPageListRequstDto dto);
    }
}
