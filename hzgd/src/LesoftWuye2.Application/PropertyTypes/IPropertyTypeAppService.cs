using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.PropertyTypes.Dto;
namespace LesoftWuye2.Application.PropertyTypes
{
    public interface IPropertyTypeAppService : IApplicationService
    {
        Task CreatePropertyType(CreatePropertyTypeInput input);
        Task DeletePropertyType(long id);
        Task UpdatePropertyType(UpdatePropertyTypeInput input);
        Task<PageListResultDto<PropertyTypeListDto>> GetPropertyTypes(GetPageListRequstDto dto);
        Task<PropertyTypeItemDto> GetPropertyType(long id);
        Task<List<ComboboxItemDto>> GetPropertyTypesForCombo();
    }
}
