using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.Activitys.Dto;using LesoftWuye2.Core.Activitys;
namespace LesoftWuye2.Application.Activitys 
 {  public class ActivityPersonAppService :LesoftWuye2.Core.ActivitysAppServiceBase,IActivityPersonAppService 
{
private readonly IRepository<ActivityPerson, long> _activitypersonRepository;public ActivityPersonAppService(IRepository<ActivityPerson, long> activitypersonRepository){_activitypersonRepository = activitypersonRepository;}
public async Task CreateActivityPerson(CreateActivityPersonInput input){var activityperson = input.MapTo<ActivityPerson>();await _activitypersonRepository.InsertAsync(activityperson);}
public async Task DeleteActivityPerson(long id){await _activitypersonRepository.DeleteAsync(id);}
public async Task UpdateActivityPerson(UpdateActivityPersonInput input){var activityperson = await _activitypersonRepository.GetAsync(input.Id);input.MapTo(activityperson);await _activitypersonRepository.UpdateAsync(activityperson);}
public async Task<PageListResultDto<ActivityPersonListDto>> GetActivityPersons(GetPageListRequstDto dto){var query = _activitypersonRepository.GetAll();var where = FilterExpression.FindByGroup<ActivityPerson>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<ActivityPersonListDto>>();return new PageListResultDto<ActivityPersonListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<ActivityPersonItemDto> GetActivityPerson(long id){var activityperson = await _activitypersonRepository.GetAsync(id);return activityperson.MapTo<ActivityPersonItemDto>();}
}
}
