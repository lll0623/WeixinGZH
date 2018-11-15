using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Wuyebase.Members.Dto;
namespace LesoftWuye2.Application.Wuyebase.Members {  public interface IMemberAppService : IApplicationService 
{
Task CreateMember(CreateMemberInput input);
Task DeleteMember(long id);
Task UpdateMember(UpdateMemberInput input);
Task<PageListResultDto<MemberListDto>> GetMembers(GetPageListRequstDto dto);
Task<MemberItemDto> GetMember(long id);
}
}
