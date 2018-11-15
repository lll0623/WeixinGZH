using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.Forums.Dto;
using LesoftWuye2.Application.Plates.Dto;
using Obs.Dto;

namespace LesoftWuye2.Application.Forums
{
    public interface IForumService : IApplicationService
    {
        Task<ForumHomeData> GetHomeData();
        Task NewPost(NewPostDto dto);

        Task<PostDetail> Detail(long id);

        Task NewComment(NewCommentDto dto);

        Task<PageListResultDto<PostListItem>> GetPostList(int plateId, GetPageListRequstDto dto);

        Task<IReadOnlyList<PlateListDto>> GetPlates();

        Task<PageListResultDto<CommentListItem>> GetCommentList(int postId, GetPageListRequstDto dto);

        Task<PageListResultDto<PostListItem>> GetMyPostList(long memberId, GetPageListRequstDto dto);
        Task<PageListResultDto<PostListItem>> GetMyCommentList(long memberId, GetPageListRequstDto dto);
    }
}
