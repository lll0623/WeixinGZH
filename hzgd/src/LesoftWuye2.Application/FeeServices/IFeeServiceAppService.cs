using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.FeeServices.Dto;
namespace LesoftWuye2.Application.FeeServices
{
    public interface IFeeServiceAppService : IApplicationService
    {
        Task CreateFeeService(CreateFeeServiceInput input);
        Task DeleteFeeService(long id);
        Task UpdateFeeService(UpdateFeeServiceInput input);
        Task<PageListResultDto<FeeServiceListDto>> GetFeeServices(GetPageListRequstDto dto);
        Task<FeeServiceItemDto> GetFeeService(long id);
    }
}
