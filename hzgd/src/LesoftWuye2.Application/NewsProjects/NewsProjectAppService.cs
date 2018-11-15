using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.NewsProjects.Dto;using LesoftWuye2.Core.NewsProjects;
namespace LesoftWuye2.Application.NewsProjects 
 {  public class NewsProjectAppService :LesoftWuye2AppServiceBase,INewsProjectAppService 
{
private readonly IRepository<NewsProject, long> _newsprojectRepository;public NewsProjectAppService(IRepository<NewsProject, long> newsprojectRepository){_newsprojectRepository = newsprojectRepository;}
public async Task CreateNewsProject(CreateNewsProjectInput input){var newsproject = input.MapTo<NewsProject>();await _newsprojectRepository.InsertAsync(newsproject);}
public async Task DeleteNewsProject(long id){await _newsprojectRepository.DeleteAsync(id);}
public async Task UpdateNewsProject(UpdateNewsProjectInput input){var newsproject = await _newsprojectRepository.GetAsync(input.Id);input.MapTo(newsproject);await _newsprojectRepository.UpdateAsync(newsproject);}
public async Task<PageListResultDto<NewsProjectListDto>> GetNewsProjects(GetPageListRequstDto dto){var query = _newsprojectRepository.GetAll();var where = FilterExpression.FindByGroup<NewsProject>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<NewsProjectListDto>>();return new PageListResultDto<NewsProjectListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<NewsProjectItemDto> GetNewsProject(long id){var newsproject = await _newsprojectRepository.GetAsync(id);return newsproject.MapTo<NewsProjectItemDto>();}
}
}
