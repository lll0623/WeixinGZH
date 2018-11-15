using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Estateinfos.Dto;
namespace LesoftWuye2.Application.Estateinfos
{
    public interface IEstateinfoAppService : IApplicationService
    {
        Task CreateEstateinfo(CreateEstateinfoInput input);
        Task DeleteEstateinfo(long id);
        Task<PageListResultDto<EstateinfoListDto>> GetEstateinfos(GetPageListRequstDto dto);
        Task<EstateinfoItemDto> GetEstateinfo(long id);

        Task ShowEstateinfo(long id);

        Task HideEstateinfo(long id);
    }
}
