using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.ProductSlideImages.Dto;
using LesoftWuye2.Application.SlideImages.Dto;

namespace LesoftWuye2.Application.ProductSlideImages
{
    public interface IProductSlideImageAppService : IApplicationService
    {
        Task CreateSlideImage(ProductCreateSlideImageInput input);
        Task DeleteSlideImage(long id);
        Task UpdateSlideImage(ProductUpdateSlideImageInput input);
        Task<PageListResultDto<ProductSlideImageListDto>> GetSlideImages(long productId, GetPageListRequstDto dto);
        Task<ProductSlideImageItemDto> GetSlideImage(long id);
    }
}
