using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.Estateinfos.Dto;using LesoftWuye2.Core.Estateinfos;
namespace LesoftWuye2.Application.Estateinfos 
 {  public class EstateinfoCommentAppService :LesoftWuye2.Core.EstateinfosAppServiceBase,IEstateinfoCommentAppService 
{
private readonly IRepository<EstateinfoComment, long> _estateinfocommentRepository;public EstateinfoCommentAppService(IRepository<EstateinfoComment, long> estateinfocommentRepository){_estateinfocommentRepository = estateinfocommentRepository;}
public async Task CreateEstateinfoComment(CreateEstateinfoCommentInput input){var estateinfocomment = input.MapTo<EstateinfoComment>();await _estateinfocommentRepository.InsertAsync(estateinfocomment);}
public async Task DeleteEstateinfoComment(long id){await _estateinfocommentRepository.DeleteAsync(id);}
public async Task UpdateEstateinfoComment(UpdateEstateinfoCommentInput input){var estateinfocomment = await _estateinfocommentRepository.GetAsync(input.Id);input.MapTo(estateinfocomment);await _estateinfocommentRepository.UpdateAsync(estateinfocomment);}
public async Task<PageListResultDto<EstateinfoCommentListDto>> GetEstateinfoComments(GetPageListRequstDto dto){var query = _estateinfocommentRepository.GetAll();var where = FilterExpression.FindByGroup<EstateinfoComment>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<EstateinfoCommentListDto>>();return new PageListResultDto<EstateinfoCommentListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<EstateinfoCommentItemDto> GetEstateinfoComment(long id){var estateinfocomment = await _estateinfocommentRepository.GetAsync(id);return estateinfocomment.MapTo<EstateinfoCommentItemDto>();}
}
}
