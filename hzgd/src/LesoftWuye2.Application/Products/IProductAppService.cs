using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Obs.Dto;
using LesoftWuye2.Application.Products.Dto;
namespace LesoftWuye2.Application.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task CreateProduct(CreateProductInput input);
        Task DeleteProduct(long id);
        Task UpdateProduct(UpdateProductInput input);
        Task<PageListResultDto<ProductListDto>> GetProducts(GetPageListRequstDto dto);
        Task<ProductItemDto> GetProduct(long id);

        Task<List<ComboboxItemDto>> GetServiceTagForCombo();
        Task<List<ComboboxItemDto>> GetProductsForCombo();


        Task<PageListResultDto<ProductCommentListDto>>GetProductComments(GetPageListRequstDto dto);
        Task DeleteProductComment(long id);
    }
}
