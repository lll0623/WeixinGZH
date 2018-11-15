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
using LesoftWuye2.Application.Newss.Dto;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.NewsProjects;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.Newss
{
    public class NewsAppService : LesoftWuye2AppServiceBase, INewsAppService
    {
        private readonly IRepository<News, long> _newsRepository;
        private readonly IRepository<NewsProject, long> _newsProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IDetailManager _detailManager;


        public NewsAppService(IRepository<News, long> newsRepository,
            IRepository<NewsProject, long> newsProjectRepository,
            IRepository<Project, long> projectRepository,
            IDetailManager detailManager)
        {
            _newsRepository = newsRepository;
            _newsProjectRepository = newsProjectRepository;
            _projectRepository = projectRepository;
            _detailManager = detailManager;
        }

        public async Task CreateNews(CreateNewsInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var news = input.MapTo<News>();
            var dataid = await _newsRepository.InsertAndGetIdAsync(news);
            _detailManager.Save(DetailType.News, dataid, input.Content);
            if (!input.AllProjects && input.Projects != null)
            {
                foreach (var project in input.Projects)
                {
                    await _newsProjectRepository.InsertAsync(new NewsProject()
                    {
                        NewsId = dataid,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task DeleteNews(long id)
        {
            await _newsProjectRepository.DeleteAsync(t => t.NewsId == id);
            await _newsRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.News, id);
        }

        public async Task UpdateNews(UpdateNewsInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var news = await _newsRepository.GetAsync(input.Id); input.MapTo(news);
            await _newsRepository.UpdateAsync(news);
            _detailManager.Save(DetailType.News, news.Id, input.Content);
            await _newsProjectRepository.DeleteAsync(t=>t.NewsId==news.Id);
            if (!input.AllProjects)
            {
                foreach (var project in input.Projects)
                {
                    await _newsProjectRepository.InsertAsync(new NewsProject()
                    {
                        NewsId = news.Id,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task<PageListResultDto<NewsListDto>> GetNewss(GetPageListRequstDto dto)
        {
            var query = _newsRepository.GetAll();
            var where = FilterExpression.FindByGroup<News>(dto.Filter);
            var count = await query.Where(where).CountAsync();

            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<NewsListDto>>();
            
            #region 包装项目名称
            foreach (var listDto in pageList)
            {
                if (listDto.AllProjects) continue;

                var q1 = _newsProjectRepository.GetAll().Where(t => t.NewsId == listDto.Id)
                 .GroupJoin(_projectRepository.GetAll(), left => left.ProjectId, right => right.Id,
                     (left, right) => new
                     {
                         NewsProjects = left,
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

            return new PageListResultDto<NewsListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<NewsItemDto> GetNews(long id)
        {
            var news = await _newsRepository.GetAsync(id);
            var dto = news.MapTo<NewsItemDto>();
            dto.Content = _detailManager.Get(DetailType.News, news.Id);
            var projects = await _newsProjectRepository.GetAll().Where(t => t.NewsId == news.Id).ToListAsync();
            dto.Projects = projects.Select(t => t.ProjectId).ToList();
            return dto;

        }
    }
}
