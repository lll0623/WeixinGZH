using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.PropertyTypes.Dto;
using LesoftWuye2.Core.PropertyTypes;
namespace LesoftWuye2.Application.PropertyTypes
{
    public class PropertyTypeAppService : LesoftWuye2AppServiceBase, IPropertyTypeAppService
    {
        private readonly IRepository<PropertyType, long> _propertytypeRepository; public PropertyTypeAppService(IRepository<PropertyType, long> propertytypeRepository) { _propertytypeRepository = propertytypeRepository; }
        public async Task CreatePropertyType(CreatePropertyTypeInput input) { var propertytype = input.MapTo<PropertyType>(); await _propertytypeRepository.InsertAsync(propertytype); }
        public async Task DeletePropertyType(long id) { await _propertytypeRepository.DeleteAsync(id); }
        public async Task UpdatePropertyType(UpdatePropertyTypeInput input) { var propertytype = await _propertytypeRepository.GetAsync(input.Id); input.MapTo(propertytype); await _propertytypeRepository.UpdateAsync(propertytype); }

        public async Task<PageListResultDto<PropertyTypeListDto>> GetPropertyTypes(GetPageListRequstDto dto)
        {
            var query = _propertytypeRepository.GetAll(); var where = FilterExpression.FindByGroup<PropertyType>(dto.Filter);
            var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync(); var pageList = list.MapTo<List<PropertyTypeListDto>>(); return new PageListResultDto<PropertyTypeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<PropertyTypeItemDto> GetPropertyType(long id) { var propertytype = await _propertytypeRepository.GetAsync(id); return propertytype.MapTo<PropertyTypeItemDto>(); }
        public async Task<List<ComboboxItemDto>> GetPropertyTypesForCombo()
        {
            var items = await _propertytypeRepository.GetAllListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }
    }
}
