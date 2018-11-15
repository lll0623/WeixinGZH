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
using LesoftWuye2.Application.PropertyCitys.Dto;
using LesoftWuye2.Core.PropertyCitys;
namespace LesoftWuye2.Application.PropertyCitys
{
    public class PropertyCityAppService : LesoftWuye2AppServiceBase, IPropertyCityAppService
    {
        private readonly IRepository<PropertyCity, long> _propertycityRepository; public PropertyCityAppService(IRepository<PropertyCity, long> propertycityRepository) { _propertycityRepository = propertycityRepository; }
        public async Task CreatePropertyCity(CreatePropertyCityInput input) { var propertycity = input.MapTo<PropertyCity>(); await _propertycityRepository.InsertAsync(propertycity); }
        public async Task DeletePropertyCity(long id) { await _propertycityRepository.DeleteAsync(id); }
        public async Task UpdatePropertyCity(UpdatePropertyCityInput input) { var propertycity = await _propertycityRepository.GetAsync(input.Id); input.MapTo(propertycity); await _propertycityRepository.UpdateAsync(propertycity); }

        public async Task<PageListResultDto<PropertyCityListDto>> GetPropertyCitys(GetPageListRequstDto dto)
        {
            var query = _propertycityRepository.GetAll();
            var where = FilterExpression.FindByGroup<PropertyCity>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<PropertyCityListDto>>();
            return new PageListResultDto<PropertyCityListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<PropertyCityItemDto> GetPropertyCity(long id) { var propertycity = await _propertycityRepository.GetAsync(id); return propertycity.MapTo<PropertyCityItemDto>(); }
        public async Task<List<ComboboxItemDto>> GetPropertyCitysForCombo()
        {
            var items = await _propertycityRepository.GetAllListAsync();
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
