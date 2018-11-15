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
using LesoftWuye2.Application.WeixinSubscribes.Dto;
using LesoftWuye2.Core.WeixinSubscribes;
namespace LesoftWuye2.Application.WeixinSubscribes
{
    public class WeixinSubscribeAppService : LesoftWuye2AppServiceBase, IWeixinSubscribeAppService
    {
        private readonly IRepository<WeixinSubscribe, long> _WeixinSubscribeRepository;

        public WeixinSubscribeAppService(IRepository<WeixinSubscribe, long> WeixinSubscribeRepository)
        {
            _WeixinSubscribeRepository = WeixinSubscribeRepository;
        }

        public async Task CreateWeixinSubscribe(CreateWeixinSubscribeInput input)
        {
            var WeixinSubscribe = input.MapTo<WeixinSubscribe>();
            await _WeixinSubscribeRepository.InsertAsync(WeixinSubscribe);
        }

        public async Task DeleteWeixinSubscribe(long id)
        {
            await _WeixinSubscribeRepository.DeleteAsync(id);
        }

        public async Task UpdateWeixinSubscribe(UpdateWeixinSubscribeInput input)
        {
            var WeixinSubscribe = await _WeixinSubscribeRepository.GetAsync(input.Id); input.MapTo(WeixinSubscribe);
            await _WeixinSubscribeRepository.UpdateAsync(WeixinSubscribe);
        }

        public async Task<PageListResultDto<WeixinSubscribeListDto>> GetWeixinSubscribes(GetPageListRequstDto dto)
        {
            var query = _WeixinSubscribeRepository.GetAll(); var where = FilterExpression.FindByGroup<WeixinSubscribe>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<WeixinSubscribeListDto>>();
            return new PageListResultDto<WeixinSubscribeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<WeixinSubscribeItemDto> GetWeixinSubscribe(long id) { var WeixinSubscribe = await _WeixinSubscribeRepository.GetAsync(id); return WeixinSubscribe.MapTo<WeixinSubscribeItemDto>(); }
    }
}
