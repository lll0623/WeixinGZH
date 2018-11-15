using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;using LesoftWuye2.Application.PayNotifys.Dto;using LesoftWuye2.Core.PayNotifys;
namespace LesoftWuye2.Application.PayNotifys 
 {  public class PayNotifyAppService :LesoftWuye2AppServiceBase,IPayNotifyAppService 
{
private readonly IRepository<PayNotify, long> _paynotifyRepository;public PayNotifyAppService(IRepository<PayNotify, long> paynotifyRepository){_paynotifyRepository = paynotifyRepository;}
public async Task CreatePayNotify(CreatePayNotifyInput input){var paynotify = input.MapTo<PayNotify>();await _paynotifyRepository.InsertAsync(paynotify);}
public async Task DeletePayNotify(long id){await _paynotifyRepository.DeleteAsync(id);}
public async Task UpdatePayNotify(UpdatePayNotifyInput input){var paynotify = await _paynotifyRepository.GetAsync(input.Id);input.MapTo(paynotify);await _paynotifyRepository.UpdateAsync(paynotify);}
public async Task<PageListResultDto<PayNotifyListDto>> GetPayNotifys(GetPageListRequstDto dto){var query = _paynotifyRepository.GetAll();var where = FilterExpression.FindByGroup<PayNotify>(dto.Filter);var count = await query.Where(where).CountAsync();var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync();var pageList = list.MapTo<List<PayNotifyListDto>>();return new PageListResultDto<PayNotifyListDto>(count, pageList, dto.CurrentPage, dto.PageSize);}
public async Task<PayNotifyItemDto> GetPayNotify(long id){var paynotify = await _paynotifyRepository.GetAsync(id);return paynotify.MapTo<PayNotifyItemDto>();}
}
}
