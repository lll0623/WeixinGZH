using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.SlideImages.Dto;
namespace LesoftWuye2.Application.SlideImages {  public interface ISlideImageAppService : IApplicationService 
{
Task CreateSlideImage(CreateSlideImageInput input);
Task DeleteSlideImage(long id);
Task UpdateSlideImage(UpdateSlideImageInput input);
Task<PageListResultDto<SlideImageListDto>> GetSlideImages(GetPageListRequstDto dto);
Task<SlideImageItemDto> GetSlideImage(long id);
}
}
