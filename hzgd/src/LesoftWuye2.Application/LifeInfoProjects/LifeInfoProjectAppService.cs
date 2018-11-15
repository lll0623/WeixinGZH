using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.LifeInfoProjects.Dto;using LesoftWuye2.Core.LifeInfoProjects;
namespace LesoftWuye2.Application.LifeInfoProjects 
 {  public class LifeInfoProjectAppService :LesoftWuye2AppServiceBase,ILifeInfoProjectAppService 
{
private readonly IRepository<LifeInfoProject, long> _lifeinfoprojectRepository;public LifeInfoProjectAppService(IRepository<LifeInfoProject, long> lifeinfoprojectRepository){_lifeinfoprojectRepository = lifeinfoprojectRepository;}
public async Task CreateLifeInfoProject(CreateLifeInfoProjectInput input){var lifeinfoproject = input.MapTo<LifeInfoProject>();await _lifeinfoprojectRepository.InsertAsync(lifeinfoproject);}
public async Task DeleteLifeInfoProject(long id){await _lifeinfoprojectRepository.DeleteAsync(id);}
public async Task UpdateLifeInfoProject(UpdateLifeInfoProjectInput input){var lifeinfoproject = await _lifeinfoprojectRepository.GetAsync(input.Id);input.MapTo(lifeinfoproject);await _lifeinfoprojectRepository.UpdateAsync(lifeinfoproject);}
public async Task<PageListResultDto<LifeInfoProjectListDto>> GetLifeInfoProjects(GetPageListRequstDto dto){var query = _lifeinfoprojectRepository.GetAll();var where = FilterExpression.FindByGroup<LifeInfoProject>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<LifeInfoProjectListDto>>();return new PageListResultDto<LifeInfoProjectListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<LifeInfoProjectItemDto> GetLifeInfoProject(long id){var lifeinfoproject = await _lifeinfoprojectRepository.GetAsync(id);return lifeinfoproject.MapTo<LifeInfoProjectItemDto>();}
}
}
