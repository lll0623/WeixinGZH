using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Propertys.Dto;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Propertys;
namespace LesoftWuye2.Application.Propertys
{
    public class PropertyAppService : LesoftWuye2AppServiceBase, IPropertyAppService
    {
        private readonly IRepository<Property, long> _propertyRepository;
        private readonly IDetailManager _detailManager;

        public PropertyAppService(IRepository<Property, long> propertyRepository, IDetailManager detailManager)
        {
            _propertyRepository = propertyRepository;
            _detailManager = detailManager;
        }

        public async Task CreateProperty(CreatePropertyInput input)
        {
            var property = input.MapTo<Property>();
            var dataId = await _propertyRepository.InsertAndGetIdAsync(property);
            _detailManager.Save(DetailType.Property, dataId, input.Content);
        }

        public async Task DeleteProperty(long id)
        {
            await _propertyRepository.DeleteAsync(id);
        }

        public async Task UpdateProperty(UpdatePropertyInput input)
        {
            var property = await _propertyRepository.GetAsync(input.Id); input.MapTo(property); await _propertyRepository.UpdateAsync(property);
        }

        public async Task<PageListResultDto<PropertyListDto>> GetPropertys(GetPageListRequstDto dto)
        {
            var query = _propertyRepository.GetAll(); var where = FilterExpression.FindByGroup<Property>(dto.Filter); var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync(); var pageList = list.MapTo<List<PropertyListDto>>(); return new PageListResultDto<PropertyListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<PropertyItemDto> GetProperty(long id)
        {
            var property = await _propertyRepository.GetAsync(id);
            var dto= property.MapTo<PropertyItemDto>();
            dto.Content = _detailManager.Get(DetailType.Property, dto.Id);
            return dto;
        }
    }
}
