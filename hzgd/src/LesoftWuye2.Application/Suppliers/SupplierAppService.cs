using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.Suppliers.Dto;
using LesoftWuye2.Core.Suppliers;
namespace LesoftWuye2.Application.Suppliers
{
    public class SupplierAppService : LesoftWuye2AppServiceBase, ISupplierAppService
    {
        private readonly IRepository<Supplier, long> _supplierRepository; public SupplierAppService(IRepository<Supplier, long> supplierRepository) { _supplierRepository = supplierRepository; }
        public async Task CreateSupplier(CreateSupplierInput input) { var supplier = input.MapTo<Supplier>(); await _supplierRepository.InsertAsync(supplier); }
        public async Task DeleteSupplier(long id) { await _supplierRepository.DeleteAsync(id); }
        public async Task UpdateSupplier(UpdateSupplierInput input) { var supplier = await _supplierRepository.GetAsync(input.Id); input.MapTo(supplier); await _supplierRepository.UpdateAsync(supplier); }
        public async Task<PageListResultDto<SupplierListDto>> GetSuppliers(GetPageListRequstDto dto) { var query = _supplierRepository.GetAll(); var where = FilterExpression.FindByGroup<Supplier>(dto.Filter); var count = await query.Where(where).CountAsync(); var list = await query.Where(where).OrderByDescending(t => t.CreationTime).PageBy(dto).ToListAsync(); var pageList = list.MapTo<List<SupplierListDto>>(); return new PageListResultDto<SupplierListDto>(count, pageList, dto.CurrentPage, dto.PageSize); }
        public async Task<SupplierItemDto> GetSupplier(long id) { var supplier = await _supplierRepository.GetAsync(id); return supplier.MapTo<SupplierItemDto>(); }
        public async Task<List<ComboboxItemDto>> GetSupplierForCombo()
        {
            var items = await _supplierRepository.GetAllListAsync();
            var list = items.Select(result =>
            {
                var item = new ComboboxItemDto();
                item.DisplayText = result.Name;
                item.Value = result.Id.ToString();
                return item;
            }).ToList();
            return list;
        }
    }
}
