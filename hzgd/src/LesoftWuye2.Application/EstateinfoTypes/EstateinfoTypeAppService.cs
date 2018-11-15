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
using LesoftWuye2.Application.EstateinfoTypes.Dto;
using LesoftWuye2.Core.EstateinfoTypes;
namespace LesoftWuye2.Application.EstateinfoTypes
{
    public class EstateinfoTypeAppService : LesoftWuye2AppServiceBase, IEstateinfoTypeAppService
    {
        private readonly IRepository<EstateinfoType, long> _lifeinfotypeRepository;

        public EstateinfoTypeAppService(IRepository<EstateinfoType, long> lifeinfotypeRepository)
        {
            _lifeinfotypeRepository = lifeinfotypeRepository;
        }

        public async Task CreateEstateinfoType(CreateEstateinfoTypeInput input)
        {
            var lifeinfotype = input.MapTo<EstateinfoType>();
            await _lifeinfotypeRepository.InsertAsync(lifeinfotype);
        }

        public async Task DeleteEstateinfoType(long id)
        {
            await _lifeinfotypeRepository.DeleteAsync(id);
        }

        public async Task UpdateEstateinfoType(UpdateEstateinfoTypeInput input)
        {
            var lifeinfotype = await _lifeinfotypeRepository.GetAsync(input.Id);
            input.MapTo(lifeinfotype);
            await _lifeinfotypeRepository.UpdateAsync(lifeinfotype);
        }

        public async Task<PageListResultDto<EstateinfoTypeListDto>> GetEstateinfoTypes(GetPageListRequstDto dto)
        {
            var query = _lifeinfotypeRepository.GetAll();
            var where = FilterExpression.FindByGroup<EstateinfoType>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<EstateinfoTypeListDto>>();
            return new PageListResultDto<EstateinfoTypeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<EstateinfoTypeItemDto> GetEstateinfoType(long id)
        {
            var lifeinfotype = await _lifeinfotypeRepository.GetAsync(id);
            return lifeinfotype.MapTo<EstateinfoTypeItemDto>();
        }
        public async Task<List<ComboboxItemDto>> GetEstateinfoTypesForCombo()
        {
            var items = await _lifeinfotypeRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
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
