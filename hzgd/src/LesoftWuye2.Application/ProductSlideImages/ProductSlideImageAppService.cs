using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using LesoftWuye2.Application.ProductSlideImages.Dto;
using Obs.Dto;
using Obs.Filter;
using LesoftWuye2.Application.SlideImages.Dto;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.SlideImages;
namespace LesoftWuye2.Application.ProductSlideImages
{
    public class ProductSlideImageAppService : LesoftWuye2AppServiceBase, IProductSlideImageAppService
    {
        private readonly IRepository<ProductSlideImage, long> _slideimageRepository;

        public ProductSlideImageAppService(IRepository<ProductSlideImage, long> slideimageRepository)
        {
            _slideimageRepository = slideimageRepository;
        }

        public async Task CreateSlideImage(ProductCreateSlideImageInput input)
        {
            var slideimage = input.MapTo<ProductSlideImage>();
            await _slideimageRepository.InsertAsync(slideimage);
        }
        public async Task DeleteSlideImage(long id) { await _slideimageRepository.DeleteAsync(id); }

        public async Task UpdateSlideImage(ProductUpdateSlideImageInput input)
        {
            var slideimage = await _slideimageRepository.GetAsync(input.Id); input.MapTo(slideimage);
            await _slideimageRepository.UpdateAsync(slideimage);
        }

        public async Task<PageListResultDto<ProductSlideImageListDto>> GetSlideImages(long productId, GetPageListRequstDto dto)
        {
            var query = _slideimageRepository.GetAll();
            var where = FilterExpression.FindByGroup<ProductSlideImage>(dto.Filter);
            where = where.And(t => t.ProductId == productId);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<ProductSlideImageListDto>>();
            return new PageListResultDto<ProductSlideImageListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }

        public async Task<ProductSlideImageItemDto> GetSlideImage(long id)
        {
            var slideimage = await _slideimageRepository.GetAsync(id);
            return slideimage.MapTo<ProductSlideImageItemDto>();
        }
    }
}
