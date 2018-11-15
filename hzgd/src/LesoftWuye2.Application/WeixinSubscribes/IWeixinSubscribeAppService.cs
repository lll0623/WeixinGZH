using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.WeixinSubscribes.Dto;
namespace LesoftWuye2.Application.WeixinSubscribes
{
    public interface IWeixinSubscribeAppService : IApplicationService
    {
        Task CreateWeixinSubscribe(CreateWeixinSubscribeInput input);
        Task DeleteWeixinSubscribe(long id);
        Task UpdateWeixinSubscribe(UpdateWeixinSubscribeInput input);
        Task<PageListResultDto<WeixinSubscribeListDto>> GetWeixinSubscribes(GetPageListRequstDto dto);
        Task<WeixinSubscribeItemDto> GetWeixinSubscribe(long id);
    }
}
