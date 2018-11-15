using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Projects.Dto;
namespace LesoftWuye2.Application.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task CreateProject(CreateProjectInput input);
        Task DeleteProject(long id);
        Task UpdateProject(UpdateProjectInput input);
        Task<PageListResultDto<ProjectListDto>> GetProjects(GetPageListRequstDto dto);
        Task<ProjectItemDto> GetProject(long id);

        Task<List<ComboboxItemDto>> GetProjectsForCombo();

        void SyncWithWuyeApi();

        
    }
}
