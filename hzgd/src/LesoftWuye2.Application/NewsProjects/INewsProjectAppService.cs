using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.NewsProjects.Dto;
namespace LesoftWuye2.Application.NewsProjects {  public interface INewsProjectAppService : IApplicationService 
{
Task CreateNewsProject(CreateNewsProjectInput input);
Task DeleteNewsProject(long id);
Task UpdateNewsProject(UpdateNewsProjectInput input);
Task<PageListResultDto<NewsProjectListDto>> GetNewsProjects(GetPageListRequstDto dto);
Task<NewsProjectItemDto> GetNewsProject(long id);
}
}
