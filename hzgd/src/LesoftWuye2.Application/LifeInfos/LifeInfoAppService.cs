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
using LesoftWuye2.Application.LifeInfos.Dto;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.LifeInfoProjects;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.NoticeProjects;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.LifeInfos
{
    public class LifeInfoAppService : LesoftWuye2AppServiceBase, ILifeInfoAppService
    {
        private readonly IRepository<LifeInfo, long> _lifeinfoRepository;
        private readonly IRepository<LifeInfoProject, long> _liefInfoProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IDetailManager _detailManager;

        public LifeInfoAppService(IRepository<LifeInfo, long> lifeinfoRepository,
            IRepository<LifeInfoProject, long> liefInfoProjectRepository,
            IRepository<Project, long> projectRepository,
            IDetailManager detailManager)
        {
            _lifeinfoRepository = lifeinfoRepository;
            _liefInfoProjectRepository = liefInfoProjectRepository;
            _projectRepository = projectRepository;
            _detailManager = detailManager;
        }

        public async Task CreateLifeInfo(CreateLifeInfoInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var lifeinfo = input.MapTo<LifeInfo>();
            var dataid = await _lifeinfoRepository.InsertAndGetIdAsync(lifeinfo);

            _detailManager.Save(DetailType.LifeInfo, dataid, input.Content);
            if (!input.AllProjects && input.Projects != null)
            {
                foreach (var project in input.Projects)
                {
                    await _liefInfoProjectRepository.InsertAsync(new LifeInfoProject()
                    {
                        LifeInfoId = dataid,
                        ProjectId = project
                    });
                }
            }



        }

        public async Task DeleteLifeInfo(long id)
        {
            await _liefInfoProjectRepository.DeleteAsync(t => t.LifeInfoId == id);
            await _lifeinfoRepository.DeleteAsync(id);

            _detailManager.Delete(DetailType.LifeInfo, id);
        }

        public async Task UpdateLifeInfo(UpdateLifeInfoInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var lifeinfo = await _lifeinfoRepository.GetAsync(input.Id);
            input.MapTo(lifeinfo);
            await _lifeinfoRepository.UpdateAsync(lifeinfo);

            _detailManager.Save(DetailType.LifeInfo, lifeinfo.Id, input.Content);
            await _liefInfoProjectRepository.DeleteAsync(t => t.LifeInfoId == lifeinfo.Id);
            if (!input.AllProjects)
            {
                foreach (var project in input.Projects)
                {
                    await _liefInfoProjectRepository.InsertAsync(new LifeInfoProject()
                    {
                        LifeInfoId = lifeinfo.Id,
                        ProjectId = project
                    });
                }
            }

        }

        public async Task<PageListResultDto<LifeInfoListDto>> GetLifeInfos(GetPageListRequstDto dto)
        {
            var query = _lifeinfoRepository.GetAll();
            var where = FilterExpression.FindByGroup<LifeInfo>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<LifeInfoListDto>>();

            #region 包装项目名称
            foreach (var listDto in pageList)
            {
                if (listDto.AllProjects) continue;

                var q1 = _liefInfoProjectRepository.GetAll().Where(t => t.LifeInfoId == listDto.Id)
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


            return new PageListResultDto<LifeInfoListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<LifeInfoItemDto> GetLifeInfo(long id)
        {
            var lifeinfo = await _lifeinfoRepository.GetAsync(id);
            var dto= lifeinfo.MapTo<LifeInfoItemDto>();

            dto.Content = _detailManager.Get(DetailType.LifeInfo, lifeinfo.Id);
            var projects = await _liefInfoProjectRepository.GetAll().Where(t => t.LifeInfoId == lifeinfo.Id).ToListAsync();
            dto.Projects = projects.Select(t => t.ProjectId).ToList();
            return dto;
        }
    }
}
