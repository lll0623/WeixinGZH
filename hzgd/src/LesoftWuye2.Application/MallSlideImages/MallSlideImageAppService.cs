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
using LesoftWuye2.Application.MallSlideImages.Dto;
using LesoftWuye2.Core.MallSlideImages;
namespace LesoftWuye2.Application.MallSlideImages
{
    public class MallSlideImageAppService : LesoftWuye2AppServiceBase, IMallSlideImageAppService
    {
        private readonly IRepository<MallSlideImage, long> _mallslideimageRepository;
        public MallSlideImageAppService(IRepository<MallSlideImage, long> mallslideimageRepository) { _mallslideimageRepository = mallslideimageRepository; }
        public async Task CreateMallSlideImage(CreateMallSlideImageInput input) { var mallslideimage = input.MapTo<MallSlideImage>(); await _mallslideimageRepository.InsertAsync(mallslideimage); }
        public async Task DeleteMallSlideImage(long id) { await _mallslideimageRepository.DeleteAsync(id); }
        public async Task UpdateMallSlideImage(UpdateMallSlideImageInput input) { var mallslideimage = await _mallslideimageRepository.GetAsync(input.Id); input.MapTo(mallslideimage); await _mallslideimageRepository.UpdateAsync(mallslideimage); }

        public async Task<PageListResultDto<MallSlideImageListDto>> GetMallSlideImages(GetPageListRequstDto dto)
        {
            var query = _mallslideimageRepository.GetAll();
            var where = FilterExpression.FindByGroup<MallSlideImage>(dto.Filter);
            var count = await query.Where(where).CountAsync();
            var list = await query.Where(where).OrderBy(t => t.Sort).PageBy(dto).ToListAsync();
            var pageList = list.MapTo<List<MallSlideImageListDto>>();
            return new PageListResultDto<MallSlideImageListDto>(count, pageList, dto.CurrentPage, dto.PageSize);
        }
        public async Task<MallSlideImageItemDto> GetMallSlideImage(long id) { var mallslideimage = await _mallslideimageRepository.GetAsync(id); return mallslideimage.MapTo<MallSlideImageItemDto>(); }
    }
}
