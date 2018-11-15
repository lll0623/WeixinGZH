using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.LifeInfoProjects.Dto;
namespace LesoftWuye2.Application.LifeInfoProjects {  public interface ILifeInfoProjectAppService : IApplicationService 
{
Task CreateLifeInfoProject(CreateLifeInfoProjectInput input);
Task DeleteLifeInfoProject(long id);
Task UpdateLifeInfoProject(UpdateLifeInfoProjectInput input);
Task<PageListResultDto<LifeInfoProjectListDto>> GetLifeInfoProjects(GetPageListRequstDto dto);
Task<LifeInfoProjectItemDto> GetLifeInfoProject(long id);
}
}
