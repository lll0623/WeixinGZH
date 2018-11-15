using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.RentsaleinfoTypes.Dto;
namespace LesoftWuye2.Application.RentsaleinfoTypes
{
    public interface IRentsaleinfoTypeAppService : IApplicationService
    {
        Task CreateRentsaleinfoType(CreateRentsaleinfoTypeInput input);
        Task DeleteRentsaleinfoType(long id);
        Task UpdateRentsaleinfoType(UpdateRentsaleinfoTypeInput input);
        Task<PageListResultDto<RentsaleinfoTypeListDto>> GetRentsaleinfoTypes(GetPageListRequstDto dto);
        Task<RentsaleinfoTypeItemDto> GetRentsaleinfoType(long id);

        Task<List<ComboboxItemDto>> GetRentsaleinfoTypesForCombo();
    }
}
