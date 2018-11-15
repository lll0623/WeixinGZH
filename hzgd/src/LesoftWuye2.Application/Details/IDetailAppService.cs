using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Details.Dto;
namespace LesoftWuye2.Application.Details {  public interface IDetailAppService : IApplicationService 
{
Task CreateDetail(CreateDetailInput input);
Task DeleteDetail(long id);
Task UpdateDetail(UpdateDetailInput input);
Task<PageListResultDto<DetailListDto>> GetDetails(GetPageListRequstDto dto);
Task<DetailItemDto> GetDetail(long id);
}
}
