using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.Rentsaleinfos.Dto;using LesoftWuye2.Core.Rentsaleinfos;
namespace LesoftWuye2.Application.Rentsaleinfos 
 {  public class RentsaleinfoImageAppService :LesoftWuye2.Core.RentsaleinfosAppServiceBase,IRentsaleinfoImageAppService 
{
private readonly IRepository<RentsaleinfoImage, long> _rentsaleinfoimageRepository;public RentsaleinfoImageAppService(IRepository<RentsaleinfoImage, long> rentsaleinfoimageRepository){_rentsaleinfoimageRepository = rentsaleinfoimageRepository;}
public async Task CreateRentsaleinfoImage(CreateRentsaleinfoImageInput input){var rentsaleinfoimage = input.MapTo<RentsaleinfoImage>();await _rentsaleinfoimageRepository.InsertAsync(rentsaleinfoimage);}
public async Task DeleteRentsaleinfoImage(long id){await _rentsaleinfoimageRepository.DeleteAsync(id);}
public async Task UpdateRentsaleinfoImage(UpdateRentsaleinfoImageInput input){var rentsaleinfoimage = await _rentsaleinfoimageRepository.GetAsync(input.Id);input.MapTo(rentsaleinfoimage);await _rentsaleinfoimageRepository.UpdateAsync(rentsaleinfoimage);}
public async Task<PageListResultDto<RentsaleinfoImageListDto>> GetRentsaleinfoImages(GetPageListRequstDto dto){var query = _rentsaleinfoimageRepository.GetAll();var where = FilterExpression.FindByGroup<RentsaleinfoImage>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<RentsaleinfoImageListDto>>();return new PageListResultDto<RentsaleinfoImageListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<RentsaleinfoImageItemDto> GetRentsaleinfoImage(long id){var rentsaleinfoimage = await _rentsaleinfoimageRepository.GetAsync(id);return rentsaleinfoimage.MapTo<RentsaleinfoImageItemDto>();}
}
}
