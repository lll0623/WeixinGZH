using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.ServiceTels.Dto;
using LesoftWuye2.Core.ServiceTels;
namespace LesoftWuye2.Application.ServiceTels
{
    public class ServiceTelAppService : LesoftWuye2AppServiceBase, IServiceTelAppService
    {
        private readonly IRepository<ServiceTel, long> _servicetelRepository; public ServiceTelAppService(IRepository<ServiceTel, long> servicetelRepository) { _servicetelRepository = servicetelRepository; }
        public async Task CreateServiceTel(CreateServiceTelInput input) { var servicetel = input.MapTo<ServiceTel>(); await _servicetelRepository.InsertAsync(servicetel); }
        public async Task DeleteServiceTel(long id) { await _servicetelRepository.DeleteAsync(id); }
        public async Task UpdateServiceTel(UpdateServiceTelInput input) { var servicetel = await _servicetelRepository.GetAsync(input.Id); input.MapTo(servicetel); await _servicetelRepository.UpdateAsync(servicetel); }

        public async Task<PageListResultDto<ServiceTelListDto>> GetServiceTels(GetPageListRequstDto dto)
        {
            var query = _servicetelRepository.GetAll(); var where = FilterExpression.FindByGroup<ServiceTel>(dto.Filter);
            var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ServiceTelListDto>>(); return new PageListResultDto<ServiceTelListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<ServiceTelItemDto> GetServiceTel(long id) { var servicetel = await _servicetelRepository.GetAsync(id); return servicetel.MapTo<ServiceTelItemDto>(); }
    }
}
