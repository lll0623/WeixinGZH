using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.LifeInfoTypes.Dto;
namespace LesoftWuye2.Application.LifeInfoTypes
{
    public interface ILifeInfoTypeAppService : IApplicationService
    {
        Task CreateLifeInfoType(CreateLifeInfoTypeInput input);
        Task DeleteLifeInfoType(long id);
        Task UpdateLifeInfoType(UpdateLifeInfoTypeInput input);
        Task<PageListResultDto<LifeInfoTypeListDto>> GetLifeInfoTypes(GetPageListRequstDto dto);
        Task<LifeInfoTypeItemDto> GetLifeInfoType(long id);

        Task<List<ComboboxItemDto>> GetLifeInfoTypesForCombo();
    }
}
