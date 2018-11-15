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
using LesoftWuye2.Application.ActivityProjects.Dto;
using LesoftWuye2.Core.ActivityProjects;
namespace LesoftWuye2.Application.ActivityProjects
{
    public class ActivityProjectAppService : LesoftWuye2AppServiceBase, IActivityProjectAppService
    {
        private readonly IRepository<ActivityProject, long> _activityprojectRepository; public ActivityProjectAppService(IRepository<ActivityProject, long> activityprojectRepository) { _activityprojectRepository = activityprojectRepository; }
        public async Task CreateActivityProject(CreateActivityProjectInput input) { var activityproject = input.MapTo<ActivityProject>(); await _activityprojectRepository.InsertAsync(activityproject); }
        public async Task DeleteActivityProject(long id) { await _activityprojectRepository.DeleteAsync(id); }
        public async Task UpdateActivityProject(UpdateActivityProjectInput input) { var activityproject = await _activityprojectRepository.GetAsync(input.Id); input.MapTo(activityproject); await _activityprojectRepository.UpdateAsync(activityproject); }
        public async Task<PageListResultDto<ActivityProjectListDto>> GetActivityProjects(GetPageListRequstDto dto) { var query = _activityprojectRepository.GetAll(); var where = FilterExpression.FindByGroup<ActivityProject>(dto.Filter); var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync(); var pageList = list.MapTo<List<ActivityProjectListDto>>(); return new PageListResultDto<ActivityProjectListDto>(count, pageList, dto.CurrentPage, dto.PageSize); }
        public async Task<ActivityProjectItemDto> GetActivityProject(long id) { var activityproject = await _activityprojectRepository.GetAsync(id); return activityproject.MapTo<ActivityProjectItemDto>(); }
    }
}
