using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.PayNotifys.Dto;
namespace LesoftWuye2.Application.PayNotifys {  public interface IPayNotifyAppService : IApplicationService 
{
Task CreatePayNotify(CreatePayNotifyInput input);
Task DeletePayNotify(long id);
Task UpdatePayNotify(UpdatePayNotifyInput input);
Task<PageListResultDto<PayNotifyListDto>> GetPayNotifys(GetPageListRequstDto dto);
Task<PayNotifyItemDto> GetPayNotify(long id);
}
}
