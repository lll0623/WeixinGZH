using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Categories.Dto;
using LesoftWuye2.Application.Plates.Dto;

namespace LesoftWuye2.Application.Plates
{
    public interface IPlateAppService : IApplicationService
    {
        Task CreatePlate(CreatePlateInput input);
        Task DeletePlate(long id);
        Task UpdatePlate(UpdatePlateInput input);
        Task<PageListResultDto<PlateListDto>> GetPlates(GetPageListRequstDto dto);
        Task<PlateItemDto> GetPlate(long id);

        Task<List<ComboboxItemDto>> GetPlateForCombo();
    }
}
