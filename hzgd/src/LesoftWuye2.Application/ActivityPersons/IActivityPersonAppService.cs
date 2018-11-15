using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Activitys.Dto;
namespace LesoftWuye2.Application.Activitys {  public interface IActivityPersonAppService : IApplicationService 
{
Task CreateActivityPerson(CreateActivityPersonInput input);
Task DeleteActivityPerson(long id);
Task UpdateActivityPerson(UpdateActivityPersonInput input);
Task<PageListResultDto<ActivityPersonListDto>> GetActivityPersons(GetPageListRequstDto dto);
Task<ActivityPersonItemDto> GetActivityPerson(long id);
}
}
