using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Estateinfos.Dto;
namespace LesoftWuye2.Application.Estateinfos {  public interface IEstateinfoImageAppService : IApplicationService 
{
Task CreateEstateinfoImage(CreateEstateinfoImageInput input);
Task DeleteEstateinfoImage(long id);
Task UpdateEstateinfoImage(UpdateEstateinfoImageInput input);
Task<PageListResultDto<EstateinfoImageListDto>> GetEstateinfoImages(GetPageListRequstDto dto);
Task<EstateinfoImageItemDto> GetEstateinfoImage(long id);
}
}
