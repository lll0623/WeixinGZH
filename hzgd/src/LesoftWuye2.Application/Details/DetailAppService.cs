using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.Details.Dto;using LesoftWuye2.Core.Details;
namespace LesoftWuye2.Application.Details 
 {  public class DetailAppService :LesoftWuye2AppServiceBase,IDetailAppService 
{
private readonly IRepository<Detail, long> _detailRepository;public DetailAppService(IRepository<Detail, long> detailRepository){_detailRepository = detailRepository;}
public async Task CreateDetail(CreateDetailInput input){var detail = input.MapTo<Detail>();await _detailRepository.InsertAsync(detail);}
public async Task DeleteDetail(long id){await _detailRepository.DeleteAsync(id);}
public async Task UpdateDetail(UpdateDetailInput input){var detail = await _detailRepository.GetAsync(input.Id);input.MapTo(detail);await _detailRepository.UpdateAsync(detail);}
public async Task<PageListResultDto<DetailListDto>> GetDetails(GetPageListRequstDto dto){var query = _detailRepository.GetAll();var where = FilterExpression.FindByGroup<Detail>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<DetailListDto>>();return new PageListResultDto<DetailListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<DetailItemDto> GetDetail(long id){var detail = await _detailRepository.GetAsync(id);return detail.MapTo<DetailItemDto>();}
}
}
