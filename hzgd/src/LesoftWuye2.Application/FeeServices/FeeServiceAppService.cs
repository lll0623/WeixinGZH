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
using LesoftWuye2.Application.FeeServices.Dto;
using LesoftWuye2.Core.Details;
using LesoftWuye2.Core.FeeServiceProjects;
using LesoftWuye2.Core.FeeServices;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Application.FeeServices
{
    public class FeeServiceAppService : LesoftWuye2AppServiceBase, IFeeServiceAppService
    {
        private readonly IRepository<FeeService, long> _FeeServiceRepository;
        private readonly IRepository<FeeServiceProject, long> _FeeServiceProjectRepository;
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IDetailManager _detailManager;


        public FeeServiceAppService(IRepository<FeeService, long> FeeServiceRepository,
            IRepository<FeeServiceProject, long> FeeServiceProjectRepository,
            IRepository<Project, long> projectRepository,
            IDetailManager detailManager)
        {
            _FeeServiceRepository = FeeServiceRepository;
            _FeeServiceProjectRepository = FeeServiceProjectRepository;
            _projectRepository = projectRepository;
            _detailManager = detailManager;
        }

        public async Task CreateFeeService(CreateFeeServiceInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }

            var FeeService = input.MapTo<FeeService>();
            var dataid = await _FeeServiceRepository.InsertAndGetIdAsync(FeeService);
            _detailManager.Save(DetailType.FeeService, dataid, input.Content);
            if (!input.AllProjects && input.Projects != null)
            {
                foreach (var project in input.Projects)
                {
                    await _FeeServiceProjectRepository.InsertAsync(new FeeServiceProject()
                    {
                        FeeServiceId = dataid,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task DeleteFeeService(long id)
        {
            await _FeeServiceProjectRepository.DeleteAsync(t => t.FeeServiceId == id);
            await _FeeServiceRepository.DeleteAsync(id);
            _detailManager.Delete(DetailType.FeeService, id);
        }

        public async Task UpdateFeeService(UpdateFeeServiceInput input)
        {
            if (!input.AllProjects && input.Projects.Count == 0)
            {
                throw new UserFriendlyException("不区分小区，所属小区至少选择一样!");
            }
            var FeeService = await _FeeServiceRepository.GetAsync(input.Id); input.MapTo(FeeService);
            await _FeeServiceRepository.UpdateAsync(FeeService);
            _detailManager.Save(DetailType.FeeService, FeeService.Id, input.Content);
            await _FeeServiceProjectRepository.DeleteAsync(t => t.FeeServiceId == FeeService.Id);
            if (!input.AllProjects)
            {
                foreach (var project in input.Projects)
                {
                    await _FeeServiceProjectRepository.InsertAsync(new FeeServiceProject()
                    {
                        FeeServiceId = FeeService.Id,
                        ProjectId = project
                    });
                }
            }
        }

        public async Task<PageListResultDto<FeeServiceListDto>> GetFeeServices(GetPageListRequstDto dto)
        {
            var query = _FeeServiceRepository.GetAll();
            var where = FilterExpression.FindByGroup<FeeService>(dto.Filter);
            var count = await query.Where(where).CountAsync();

            var list = await query
                .Where(where)
                .OrderByDescending(t => t.CreationTime)
                .PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<FeeServiceListDto>>();

            #region 包装项目名称
            foreach (var listDto in pageList)
            {
                if (listDto.AllProjects) continue;

                var q1 = _FeeServiceProjectRepository.GetAll().Where(t => t.FeeServiceId == listDto.Id)
                 .GroupJoin(_projectRepository.GetAll(), left => left.ProjectId, right => right.Id,
                     (left, right) => new
                     {
                         FeeServiceProjects = left,
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

            return new PageListResultDto<FeeServiceListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<FeeServiceItemDto> GetFeeService(long id)
        {
            var FeeService = await _FeeServiceRepository.GetAsync(id);
            var dto = FeeService.MapTo<FeeServiceItemDto>();
            dto.Content = _detailManager.Get(DetailType.FeeService, FeeService.Id);
            var projects = await _FeeServiceProjectRepository.GetAll().Where(t => t.FeeServiceId == FeeService.Id).ToListAsync();
            dto.Projects = projects.Select(t => t.ProjectId).ToList();
            return dto;

        }
    }
}
