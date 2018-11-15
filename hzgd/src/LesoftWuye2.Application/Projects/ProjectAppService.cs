using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Projects.Dto;
using LesoftWuye2.Application.WuyeApis;
using LesoftWuye2.Core.Projects;
namespace LesoftWuye2.Application.Projects
{
    public class ProjectAppService : LesoftWuye2AppServiceBase, IProjectAppService
    {
        private readonly IRepository<Project, long> _projectRepository;
        private readonly IWuyeApiAppSrvice _wuyeApiAppSrvice;

        public ProjectAppService(IRepository<Project, long> projectRepository, IWuyeApiAppSrvice wuyeApiAppSrvice)
        {
            _projectRepository = projectRepository;
            _wuyeApiAppSrvice = wuyeApiAppSrvice;
        }

        public async Task CreateProject(CreateProjectInput input)
        {
            if (await _projectRepository.CountAsync(t => t.Code == input.Code) > 0)
            {
                throw new UserFriendlyException($"小区代号:【{input.Code}】已经存在!");
            }

            var project = input.MapTo<Project>();
            await _projectRepository.InsertAsync(project);
        }

        public async Task DeleteProject(long id)
        {
            await _projectRepository.DeleteAsync(id);
        }

        public async Task UpdateProject(UpdateProjectInput input)
        {
            if (await _projectRepository.CountAsync(t => t.Code == input.Code && t.Id != input.Id) > 0)
            {
                throw new UserFriendlyException($"小区代号:【{input.Code}】已经存在!");
            }
            var project = await _projectRepository.GetAsync(input.Id);
            input.MapTo(project);

            await _projectRepository.UpdateAsync(project);
        }

        public async Task<PageListResultDto<ProjectListDto>> GetProjects(GetPageListRequstDto dto)
        {
            var query = _projectRepository.GetAll();
            var where = FilterExpression.FindByGroup<Project>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Code).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ProjectListDto>>();
            return new PageListResultDto<ProjectListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<ProjectItemDto> GetProject(long id)
        {
            var project = await _projectRepository.GetAsync(id);
            return project.MapTo<ProjectItemDto>();
        }

        public async Task<List<ComboboxItemDto>> GetProjectsForCombo()
        {
            var items = await _projectRepository.GetAllListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }

        public void SyncWithWuyeApi()
        {
            var result = _wuyeApiAppSrvice.GetPProjectList();
            if (result.Success)
            {
                if (result.Records != null)
                {
                    foreach (var item in result.Records)
                    {
                        var data = _projectRepository.FirstOrDefault(t => t.Code == item.Code);
                        if (data == null)
                        {
                            _projectRepository.Insert(new Project() {Code = item.Code, Name = item.Name});
                        }
                        else
                        {
                            data.Name = item.Name;
                            _projectRepository.Update(data);
                        }
                    }
                }
            }
            else
            {
                throw new UserFriendlyException(result.msg);
            }
        }
    }
}
