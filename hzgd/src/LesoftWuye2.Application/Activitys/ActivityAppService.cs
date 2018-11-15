using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Activitys.Dto;
using LesoftWuye2.Core.ActivityProjects;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Activitys
{
    public class ActivityAppService : LesoftWuye2AppServiceBase, IActivityAppService
    {
        private readonly IRepository<Activity, long> _activityRepository;
        private readonly IRepository<ActivityPerson, long> _activityPersonRepository;
        private readonly IRepository<ActivityProject, long> _activityProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IDetailManager _detailManager;


        public ActivityAppService(IRepository<Activity, long> activityRepository,
            IRepository<ActivityPerson, long> activityPersonRepository,
             IRepository<ActivityProject, long> activityProjectRepository,
            IRepository<Project, long> projectRepository,
            IDetailManager detailManager)
        {
            _activityRepository = activityRepository;
            _activityPersonRepository = activityPersonRepository;
            _activityProjectRepository = activityProjectRepository;
            _projectRepository = projectRepository;
            _detailManager = detailManager;
        }

        public async Task CreateActivity(CreateActivityInput input)
        {
            //input.Expireday=DateTime.Now;

            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var activity = input.MapTo<Activity>();
            var dataid = await _activityRepository.InsertAndGetIdAsync(activity);
            _detailManager.Save(DetailType.Activity, dataid, input.Content);
            if (!input.AllProjects && input.Projects != null)
            {
                foreach (var project in input.Projects)
                {
                    await _activityProjectRepository.InsertAsync(new ActivityProject()
                    {
                        ActivityId = dataid,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task DeleteActivity(long id)
        {
            await _activityProjectRepository.DeleteAsync(t => t.ActivityId ==id);
            await _activityPersonRepository.DeleteAsync(t => t.ActivityId == id);
            await _activityRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.Activity, id);
        }

        public async Task UpdateActivity(UpdateActivityInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var activity = await _activityRepository.GetAsync(input.Id);
            input.MapTo(activity); await _activityRepository.UpdateAsync(activity);
            _detailManager.Save(DetailType.Activity, activity.Id, input.Content);
            await _activityProjectRepository.DeleteAsync(t => t.ActivityId == activity.Id);
            if (!input.AllProjects)
            {
                foreach (var project in input.Projects)
                {
                    await _activityProjectRepository.InsertAsync(new ActivityProject()
                    {
                        ActivityId = activity.Id,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task<PageListResultDto<ActivityListDto>> GetActivitys(GetPageListRequstDto dto)
        {
            var query = _activityRepository.GetAll(); var where = FilterExpression.FindByGroup<Activity>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ActivityListDto>>();

            #region 包装项目名称
            foreach (var listDto in pageList)
            {
                if (listDto.AllProjects) continue;

                var q1 = _activityProjectRepository.GetAll().Where(t => t.ActivityId == listDto.Id)
                 .GroupJoin(_projectRepository.GetAll(), left => left.ProjectId, right => right.Id,
                     (left, right) => new
                     {
                         ActivityProjects = left,
                         Project = right
                     });
                var projects = await q1.ToListAsync();
                var pnames = projects.Select(t =>
                {
                    var firstOrDefault = t.Project.FirstOrDefault();
                    if (firstOrDefault != null)
                        return firstOrDefault.Name;
                    else
                    {
                        return "";
                    }
                }).ToList();
                listDto.ProjectNames = pnames;
            }
            #endregion


            return new PageListResultDto<ActivityListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<ActivityItemDto> GetActivity(long id)
        {
            var activity = await _activityRepository.GetAsync(id);
            var dto= activity.MapTo<ActivityItemDto>();
            dto.Content = _detailManager.Get(DetailType.Activity, activity.Id);
            var projects = await _activityProjectRepository.GetAll().Where(t => t.ActivityId == activity.Id).ToListAsync();
            dto.Projects = projects.Select(t => t.ProjectId).ToList();
            return dto;
        }

        public async Task<PageListResultDto<ActivityPersonListDto>> GetActivityPersons(long activityId, GetPageListRequstDto dto)
        {
            var query = _activityPersonRepository.GetAll();
            var where = FilterExpression.FindByGroup<ActivityPerson>(dto.Filter);
            where = where.And(t => t.ActivityId == activityId);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ActivityPersonListDto>>();

            return new PageListResultDto<ActivityPersonListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
    }
}
