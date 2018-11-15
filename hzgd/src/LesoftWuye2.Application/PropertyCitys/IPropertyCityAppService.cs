using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.PropertyCitys.Dto;
namespace LesoftWuye2.Application.PropertyCitys
{
    public interface IPropertyCityAppService : IApplicationService
    {
        Task CreatePropertyCity(CreatePropertyCityInput input);
        Task DeletePropertyCity(long id);
        Task UpdatePropertyCity(UpdatePropertyCityInput input);
        Task<PageListResultDto<PropertyCityListDto>> GetPropertyCitys(GetPageListRequstDto dto);
        Task<PropertyCityItemDto> GetPropertyCity(long id);

        Task<List<ComboboxItemDto>> GetPropertyCitysForCombo();
    }
}
