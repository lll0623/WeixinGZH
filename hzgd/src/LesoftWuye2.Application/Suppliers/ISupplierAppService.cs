using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Suppliers.Dto;
namespace LesoftWuye2.Application.Suppliers
{
    public interface ISupplierAppService : IApplicationService
    {
        Task CreateSupplier(CreateSupplierInput input);
        Task DeleteSupplier(long id);
        Task UpdateSupplier(UpdateSupplierInput input);
        Task<PageListResultDto<SupplierListDto>> GetSuppliers(GetPageListRequstDto dto);
        Task<SupplierItemDto> GetSupplier(long id);

        Task<List<ComboboxItemDto>> GetSupplierForCombo();
    }
}
