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
using LesoftWuye2.Application.LifeInfoTypes.Dto;
using LesoftWuye2.Core.LifeInfoTypes;
namespace LesoftWuye2.Application.LifeInfoTypes
{
    public class LifeInfoTypeAppService : LesoftWuye2AppServiceBase, ILifeInfoTypeAppService
    {
        private readonly IRepository<LifeInfoType, long> _lifeinfotypeRepository; public LifeInfoTypeAppService(IRepository<LifeInfoType, long> lifeinfotypeRepository) { _lifeinfotypeRepository = lifeinfotypeRepository; }

        public async Task CreateLifeInfoType(CreateLifeInfoTypeInput input)
        {
            var lifeinfotype = input.MapTo<LifeInfoType>(); await _lifeinfotypeRepository.InsertAsync(lifeinfotype);
        }
        public async Task DeleteLifeInfoType(long id) { await _lifeinfotypeRepository.DeleteAsync(id); }

        public async Task UpdateLifeInfoType(UpdateLifeInfoTypeInput input)
        {
            var lifeinfotype = await _lifeinfotypeRepository.GetAsync(input.Id);
            input.MapTo(lifeinfotype);
            await _lifeinfotypeRepository.UpdateAsync(lifeinfotype);
        }

        public async Task<PageListResultDto<LifeInfoTypeListDto>> GetLifeInfoTypes(GetPageListRequstDto dto)
        {
            var query = _lifeinfotypeRepository.GetAll(); var where = FilterExpression.FindByGroup<LifeInfoType>(dto.Filter);
            var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<LifeInfoTypeListDto>>(); return new PageListResultDto<LifeInfoTypeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<LifeInfoTypeItemDto> GetLifeInfoType(long id) { var lifeinfotype = await _lifeinfotypeRepository.GetAsync(id); return lifeinfotype.MapTo<LifeInfoTypeItemDto>(); }
        public async Task<List<ComboboxItemDto>> GetLifeInfoTypesForCombo()
        {
            var items = await _lifeinfotypeRepository.GetAllListAsync();
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
