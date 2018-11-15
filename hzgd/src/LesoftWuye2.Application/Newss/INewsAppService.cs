using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Newss.Dto;
namespace LesoftWuye2.Application.Newss
{
    public interface INewsAppService : IApplicationService
    {
        Task CreateNews(CreateNewsInput input);
        Task DeleteNews(long id);
        Task UpdateNews(UpdateNewsInput input);
        Task<PageListResultDto<NewsListDto>> GetNewss(GetPageListRequstDto dto);
        Task<NewsItemDto> GetNews(long id);
    }
}
