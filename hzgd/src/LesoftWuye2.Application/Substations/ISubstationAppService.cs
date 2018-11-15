using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Substations.Dto;
namespace LesoftWuye2.Application.Substations
{
    public interface ISubstationAppService : IApplicationService
    {
        Task CreateSubstation(CreateSubstationInput input);
        Task DeleteSubstation(long id);
        Task UpdateSubstation(UpdateSubstationInput input);
        Task<PageListResultDto<SubstationListDto>> GetSubstations(GetPageListRequstDto dto);
        Task<SubstationItemDto> GetSubstation(long id);
    }
}
