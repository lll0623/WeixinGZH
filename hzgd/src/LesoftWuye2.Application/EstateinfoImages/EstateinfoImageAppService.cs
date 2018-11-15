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
 {  public class EstateinfoImageAppService :LesoftWuye2.Core.EstateinfosAppServiceBase,IEstateinfoImageAppService 
{
private readonly IRepository<EstateinfoImage, long> _estateinfoimageRepository;public EstateinfoImageAppService(IRepository<EstateinfoImage, long> estateinfoimageRepository){_estateinfoimageRepository = estateinfoimageRepository;}
public async Task CreateEstateinfoImage(CreateEstateinfoImageInput input){var estateinfoimage = input.MapTo<EstateinfoImage>();await _estateinfoimageRepository.InsertAsync(estateinfoimage);}
public async Task DeleteEstateinfoImage(long id){await _estateinfoimageRepository.DeleteAsync(id);}
public async Task UpdateEstateinfoImage(UpdateEstateinfoImageInput input){var estateinfoimage = await _estateinfoimageRepository.GetAsync(input.Id);input.MapTo(estateinfoimage);await _estateinfoimageRepository.UpdateAsync(estateinfoimage);}
public async Task<PageListResultDto<EstateinfoImageListDto>> GetEstateinfoImages(GetPageListRequstDto dto){var query = _estateinfoimageRepository.GetAll();var where = FilterExpression.FindByGroup<EstateinfoImage>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<EstateinfoImageListDto>>();return new PageListResultDto<EstateinfoImageListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<EstateinfoImageItemDto> GetEstateinfoImage(long id){var estateinfoimage = await _estateinfoimageRepository.GetAsync(id);return estateinfoimage.MapTo<EstateinfoImageItemDto>();}
}
}
