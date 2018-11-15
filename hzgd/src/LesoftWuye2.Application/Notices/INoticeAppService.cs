using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Notices.Dto;
namespace LesoftWuye2.Application.Notices
{
    public interface INoticeAppService : IApplicationService
    {
        Task CreateNotice(CreateNoticeInput input);
        Task DeleteNotice(long id);
        Task UpdateNotice(UpdateNoticeInput input);
        Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto);
        Task<NoticeItemDto> GetNotice(long id);
    }
}
