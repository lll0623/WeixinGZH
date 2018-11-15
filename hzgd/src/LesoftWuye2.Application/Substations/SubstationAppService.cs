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
using LesoftWuye2.Application.Substations.Dto;
using LesoftWuye2.Core.Substations;
namespace LesoftWuye2.Application.Substations
{
    public class SubstationAppService : LesoftWuye2AppServiceBase, ISubstationAppService
    {
        private readonly IRepository<Substation, long> _substationRepository; public SubstationAppService(IRepository<Substation, long> substationRepository) { _substationRepository = substationRepository; }
        public async Task CreateSubstation(CreateSubstationInput input) { var substation = input.MapTo<Substation>(); await _substationRepository.InsertAsync(substation); }
        public async Task DeleteSubstation(long id) { await _substationRepository.DeleteAsync(id); }
        public async Task UpdateSubstation(UpdateSubstationInput input) { var substation = await _substationRepository.GetAsync(input.Id); input.MapTo(substation); await _substationRepository.UpdateAsync(substation); }

        public async Task<PageListResultDto<SubstationListDto>> GetSubstations(GetPageListRequstDto dto)
        {
            var query = _substationRepository.GetAll(); var where = FilterExpression.FindByGroup<Substation>(dto.Filter);
            var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<SubstationListDto>>(); return new PageListResultDto<SubstationListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<SubstationItemDto> GetSubstation(long id) { var substation = await _substationRepository.GetAsync(id); return substation.MapTo<SubstationItemDto>(); }
    }
}
