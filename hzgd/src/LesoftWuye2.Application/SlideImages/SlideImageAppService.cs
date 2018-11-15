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
using LesoftWuye2.Application.SlideImages.Dto;
using LesoftWuye2.Core.SlideImages;
namespace LesoftWuye2.Application.SlideImages
{
    public class SlideImageAppService : LesoftWuye2AppServiceBase, ISlideImageAppService
    {
        private readonly IRepository<SlideImage, long> _slideimageRepository; public SlideImageAppService(IRepository<SlideImage, long> slideimageRepository) { _slideimageRepository = slideimageRepository; }
        public async Task CreateSlideImage(CreateSlideImageInput input) { var slideimage = input.MapTo<SlideImage>(); await _slideimageRepository.InsertAsync(slideimage); }
        public async Task DeleteSlideImage(long id) { await _slideimageRepository.DeleteAsync(id); }
        public async Task UpdateSlideImage(UpdateSlideImageInput input) { var slideimage = await _slideimageRepository.GetAsync(input.Id); input.MapTo(slideimage); await _slideimageRepository.UpdateAsync(slideimage); }

        public async Task<PageListResultDto<SlideImageListDto>> GetSlideImages(GetPageListRequstDto dto)
        {
            var query = _slideimageRepository.GetAll();
            var where = FilterExpression.FindByGroup<SlideImage>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderByDescending(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<SlideImageListDto>>();
            return new PageListResultDto<SlideImageListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<SlideImageItemDto> GetSlideImage(long id) { var slideimage = await _slideimageRepository.GetAsync(id); return slideimage.MapTo<SlideImageItemDto>(); }
    }
}
