using System.Threading.Tasks;
                            using Abp.Application.Services;
                            using Obs.Dto;
                            using LesoftWuye2.Application.Estateinfos.Dto;
namespace LesoftWuye2.Application.Estateinfos {  public interface IEstateinfoCommentAppService : IApplicationService 
{
Task CreateEstateinfoComment(CreateEstateinfoCommentInput input);
Task DeleteEstateinfoComment(long id);
Task UpdateEstateinfoComment(UpdateEstateinfoCommentInput input);
Task<PageListResultDto<EstateinfoCommentListDto>> GetEstateinfoComments(GetPageListRequstDto dto);
Task<EstateinfoCommentItemDto> GetEstateinfoComment(long id);
}
}
