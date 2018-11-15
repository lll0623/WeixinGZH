using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.MallSlideImages.Dto;
namespace LesoftWuye2.Application.MallSlideImages {  public interface IMallSlideImageAppService : IApplicationService 
{
Task CreateMallSlideImage(CreateMallSlideImageInput input);
Task DeleteMallSlideImage(long id);
Task UpdateMallSlideImage(UpdateMallSlideImageInput input);
Task<PageListResultDto<MallSlideImageListDto>> GetMallSlideImages(GetPageListRequstDto dto);
Task<MallSlideImageItemDto> GetMallSlideImage(long id);
}
}
