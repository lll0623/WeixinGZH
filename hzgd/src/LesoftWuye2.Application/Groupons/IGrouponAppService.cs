using System.Threading.Tasks;
using Abp.Application.Services;
using Obs.Dto;
using LesoftWuye2.Application.Groupons.Dto;
namespace LesoftWuye2.Application.Groupons
{
    public interface IGrouponAppService : IApplicationService
    {
        Task CreateGroupon(CreateGrouponInput input);
        Task DeleteGroupon(long id);
        Task UpdateGroupon(UpdateGrouponInput input);
        Task<PageListResultDto<GrouponListDto>> GetGroupons(GetPageListRequstDto dto);
        Task<GrouponItemDto> GetGroupon(long id);
    }
}
