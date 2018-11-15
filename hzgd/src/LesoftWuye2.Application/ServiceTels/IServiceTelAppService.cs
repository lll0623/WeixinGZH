using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.ServiceTels.Dto;
namespace LesoftWuye2.Application.ServiceTels {  public interface IServiceTelAppService : IApplicationService 
{
Task CreateServiceTel(CreateServiceTelInput input);
Task DeleteServiceTel(long id);
Task UpdateServiceTel(UpdateServiceTelInput input);
Task<PageListResultDto<ServiceTelListDto>> GetServiceTels(GetPageListRequstDto dto);
Task<ServiceTelItemDto> GetServiceTel(long id);
}
}
