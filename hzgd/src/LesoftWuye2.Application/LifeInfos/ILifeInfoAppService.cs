using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.LifeInfos.Dto;
namespace LesoftWuye2.Application.LifeInfos {  public interface ILifeInfoAppService : IApplicationService 
{
Task CreateLifeInfo(CreateLifeInfoInput input);
Task DeleteLifeInfo(long id);
Task UpdateLifeInfo(UpdateLifeInfoInput input);
Task<PageListResultDto<LifeInfoListDto>> GetLifeInfos(GetPageListRequstDto dto);
Task<LifeInfoItemDto> GetLifeInfo(long id);
}
}
