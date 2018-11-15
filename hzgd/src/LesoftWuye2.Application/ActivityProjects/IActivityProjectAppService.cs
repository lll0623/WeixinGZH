using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.ActivityProjects.Dto;
namespace LesoftWuye2.Application.ActivityProjects
{
    public interface IActivityProjectAppService : IApplicationService
    {
        Task CreateActivityProject(CreateActivityProjectInput input);
        Task DeleteActivityProject(long id);
        Task UpdateActivityProject(UpdateActivityProjectInput input);
        Task<PageListResultDto<ActivityProjectListDto>> GetActivityProjects(GetPageListRequstDto dto);
        Task<ActivityProjectItemDto> GetActivityProject(long id);
      
    }
}
