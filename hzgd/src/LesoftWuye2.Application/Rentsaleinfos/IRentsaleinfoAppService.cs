using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Rentsaleinfos.Dto;
namespace LesoftWuye2.Application.Rentsaleinfos
{
    public interface IRentsaleinfoAppService : IApplicationService
    {
        Task CreateRentsaleinfo(CreateRentsaleinfoInput input);
        Task DeleteRentsaleinfo(long id);
        Task<PageListResultDto<RentsaleinfoListDto>> GetRentsaleinfos(GetPageListRequstDto dto);
        Task<RentsaleinfoItemDto> GetRentsaleinfo(long id);

        Task ShowRentsaleinfo(long id);

        Task HideRentsaleinfo(long id);

        Task UpdateRentsaleinfo(UpdateRentsaleinfoInput input);
    }
}
