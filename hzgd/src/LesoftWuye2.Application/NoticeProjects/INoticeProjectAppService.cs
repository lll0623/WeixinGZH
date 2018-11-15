using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.NoticeProjects.Dto;
namespace LesoftWuye2.Application.NoticeProjects {  public interface INoticeProjectAppService : IApplicationService 
{
Task CreateNoticeProject(CreateNoticeProjectInput input);
Task DeleteNoticeProject(long id);
Task UpdateNoticeProject(UpdateNoticeProjectInput input);
Task<PageListResultDto<NoticeProjectListDto>> GetNoticeProjects(GetPageListRequstDto dto);
Task<NoticeProjectItemDto> GetNoticeProject(long id);
}
}
