using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.EstateinfoTypes.Dto;
namespace LesoftWuye2.Application.EstateinfoTypes
{
    public interface IEstateinfoTypeAppService : IApplicationService
    {
        Task CreateEstateinfoType(CreateEstateinfoTypeInput input);
        Task DeleteEstateinfoType(long id);
        Task UpdateEstateinfoType(UpdateEstateinfoTypeInput input);
        Task<PageListResultDto<EstateinfoTypeListDto>> GetEstateinfoTypes(GetPageListRequstDto dto);
        Task<EstateinfoTypeItemDto> GetEstateinfoType(long id);

        Task<List<ComboboxItemDto>> GetEstateinfoTypesForCombo();
    }
}
