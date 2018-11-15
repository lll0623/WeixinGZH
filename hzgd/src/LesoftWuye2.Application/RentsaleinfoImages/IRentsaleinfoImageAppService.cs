using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Rentsaleinfos.Dto;
namespace LesoftWuye2.Application.Rentsaleinfos {  public interface IRentsaleinfoImageAppService : IApplicationService 
{
Task CreateRentsaleinfoImage(CreateRentsaleinfoImageInput input);
Task DeleteRentsaleinfoImage(long id);
Task UpdateRentsaleinfoImage(UpdateRentsaleinfoImageInput input);
Task<PageListResultDto<RentsaleinfoImageListDto>> GetRentsaleinfoImages(GetPageListRequstDto dto);
Task<RentsaleinfoImageItemDto> GetRentsaleinfoImage(long id);
}
}
