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
using LesoftWuye2.Application.Notices.Dto;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Notices
{
    public class NoticeAppService : LesoftWuye2AppServiceBase, INoticeAppService
    {
        private readonly IRepository<Notice, long> _noticeRepository;
        private readonly IRepository<NoticeProject, long> _noticeProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IDetailManager _detailManager;


        public NoticeAppService(IRepository<Notice, long> noticeRepository,
            IRepository<NoticeProject, long> noticeProjectRepository,
            IRepository<Project, long> projectRepository,
            IDetailManager detailManager)
        {
            _noticeRepository = noticeRepository;
            _noticeProjectRepository = noticeProjectRepository;
            _projectRepository = projectRepository;
            _detailManager = detailManager;
        }

        public async Task CreateNotice(CreateNoticeInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var notice = input.MapTo<Notice>();
            var dataid = await _noticeRepository.InsertAndGetIdAsync(notice);
            _detailManager.Save(DetailType.Notice, dataid, input.Content);
            if (!input.AllProjects && input.Projects != null)
            {
                foreach (var project in input.Projects)
                {
                    await _noticeProjectRepository.InsertAsync(new NoticeProject()
                    {
                        NoticeId = dataid,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task DeleteNotice(long id)
        {
            await _noticeProjectRepository.DeleteAsync(t => t.NoticeId == id);
            await _noticeRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.Notice, id);
        }

        public async Task UpdateNotice(UpdateNoticeInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var notice = await _noticeRepository.GetAsync(input.Id); input.MapTo(notice);
            await _noticeRepository.UpdateAsync(notice);
            _detailManager.Save(DetailType.Notice, notice.Id, input.Content);
            await _noticeProjectRepository.DeleteAsync(t => t.NoticeId == notice.Id);
            if (!input.AllProjects)
            {
                foreach (var project in input.Projects)
                {
                    await _noticeProjectRepository.InsertAsync(new NoticeProject()
                    {
                        NoticeId = notice.Id,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task<PageListResultDto<NoticeListDto>> GetNotices(GetPageListRequstDto dto)
        {
            var query = _noticeRepository.GetAll();
            var where = FilterExpression.FindByGroup<Notice>(dto.Filter);
            var count = await query.Where(where).CountAsync();

            var list = await query
                .Where(where)
                .OrderByDescending(t => t.Sort)
                .PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<NoticeListDto>>();

            #region 包装项目名称
            foreach (var listDto in pageList)
            {
                if (listDto.AllProjects) continue;

                var q1 = _noticeProjectRepository.GetAll().Where(t => t.NoticeId == listDto.Id)
                 .GroupJoin(_projectRepository.GetAll(), left => left.ProjectId, right => right.Id,
                     (left, right) => new
                     {
                         NoticeProjects = left,
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

            return new PageListResultDto<NoticeListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<NoticeItemDto> GetNotice(long id)
        {
            var notice = await _noticeRepository.GetAsync(id);
            var dto = notice.MapTo<NoticeItemDto>();
            dto.Content = _detailManager.Get(DetailType.Notice, notice.Id);
            var projects = await _noticeProjectRepository.GetAll().Where(t => t.NoticeId == notice.Id).ToListAsync();
            dto.Projects = projects.Select(t => t.ProjectId).ToList();
            return dto;

        }
    }
}
