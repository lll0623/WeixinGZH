using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.NoticeProjects.Dto;using LesoftWuye2.Core.NoticeProjects;
namespace LesoftWuye2.Application.NoticeProjects 
 {  public class NoticeProjectAppService :LesoftWuye2AppServiceBase,INoticeProjectAppService 
{
private readonly IRepository<NoticeProject, long> _noticeprojectRepository;public NoticeProjectAppService(IRepository<NoticeProject, long> noticeprojectRepository){_noticeprojectRepository = noticeprojectRepository;}
public async Task CreateNoticeProject(CreateNoticeProjectInput input){var noticeproject = input.MapTo<NoticeProject>();await _noticeprojectRepository.InsertAsync(noticeproject);}
public async Task DeleteNoticeProject(long id){await _noticeprojectRepository.DeleteAsync(id);}
public async Task UpdateNoticeProject(UpdateNoticeProjectInput input){var noticeproject = await _noticeprojectRepository.GetAsync(input.Id);input.MapTo(noticeproject);await _noticeprojectRepository.UpdateAsync(noticeproject);}
public async Task<PageListResultDto<NoticeProjectListDto>> GetNoticeProjects(GetPageListRequstDto dto){var query = _noticeprojectRepository.GetAll();var where = FilterExpression.FindByGroup<NoticeProject>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<NoticeProjectListDto>>();return new PageListResultDto<NoticeProjectListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<NoticeProjectItemDto> GetNoticeProject(long id){var noticeproject = await _noticeprojectRepository.GetAsync(id);return noticeproject.MapTo<NoticeProjectItemDto>();}
}
}
