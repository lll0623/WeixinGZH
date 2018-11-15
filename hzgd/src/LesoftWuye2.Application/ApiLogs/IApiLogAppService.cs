using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.ApiLogs.Dto;
using Obs.Dto;

namespace LesoftWuye2.Application.ApiLogs
{
    public interface IApiLogAppService : IApplicationService
    {
        Task CreateApiLog(CreateApiLogInput input);
        Task DeleteApiLog(long id);
        Task UpdateApiLog(UpdateApiLogInput input);
        Task<PageListResultDto<ApiLogListDto>> GetApiLogs(GetPageListRequstDto dto);
        Task<ApiLogItemDto> GetApiLog(long id);
        Task ClearAll();

    }
}
