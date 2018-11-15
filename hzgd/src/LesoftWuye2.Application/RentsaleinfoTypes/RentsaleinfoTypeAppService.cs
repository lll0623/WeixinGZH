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
using LesoftWuye2.Application.RentsaleinfoTypes.Dto;
using LesoftWuye2.Core.RentsaleinfoTypes;
namespace LesoftWuye2.Application.RentsaleinfoTypes
{
    public class RentsaleinfoTypeAppService : LesoftWuye2AppServiceBase, IRentsaleinfoTypeAppService
    {
        private readonly IRepository<RentsaleinfoType, long> _rentsaleinfotypeRepository;

        public RentsaleinfoTypeAppService(IRepository<RentsaleinfoType, long> rentsaleinfotypeRepository)
        {
            _rentsaleinfotypeRepository = rentsaleinfotypeRepository;
        }

        public async Task CreateRentsaleinfoType(CreateRentsaleinfoTypeInput input)
        {
            var rentsaleinfotype = input.MapTo<RentsaleinfoType>();
            await _rentsaleinfotypeRepository.InsertAsync(rentsaleinfotype);
        }

        public async Task DeleteRentsaleinfoType(long id)
        {
            await _rentsaleinfotypeRepository.DeleteAsync(id);
        }

        public async Task UpdateRentsaleinfoType(UpdateRentsaleinfoTypeInput input)
        {
            var rentsaleinfotype = await _rentsaleinfotypeRepository.GetAsync(input.Id);
            input.MapTo(rentsaleinfotype);
            await _rentsaleinfotypeRepository.UpdateAsync(rentsaleinfotype);
        }

        public async Task<PageListResultDto<RentsaleinfoTypeListDto>> GetRentsaleinfoTypes(GetPageListRequstDto dto)
        {
            var query = _rentsaleinfotypeRepository.GetAll();
            var where = FilterExpression.FindByGroup<RentsaleinfoType>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<RentsaleinfoTypeListDto>>();
            return new PageListResultDto<RentsaleinfoTypeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<RentsaleinfoTypeItemDto> GetRentsaleinfoType(long id)
        {
            var rentsaleinfotype = await _rentsaleinfotypeRepository.GetAsync(id);
            return rentsaleinfotype.MapTo<RentsaleinfoTypeItemDto>();
        }
        public async Task<List<ComboboxItemDto>> GetRentsaleinfoTypesForCombo()
        {
            var items = await _rentsaleinfotypeRepository.GetAll().OrderByDescending(t => t.Sort).ToListAsync();
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
