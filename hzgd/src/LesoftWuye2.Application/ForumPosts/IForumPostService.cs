using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using LesoftWuye2.Application.ForumPosts.Dto;
using Obs.Dto;

namespace LesoftWuye2.Application.ForumPosts
{
    public interface IForumPostService : IApplicationService
    {
        Task<PageListResultDto<ForumPostListDto>> GetForumPosts(GetPageListRequstDto dto);

        void DeleteForumPost(long id);

        Task<ForumPostItemDto> GetForumPost(long id);

        void DeleteForumComment(long id);

        Task ForumPostReply(ForumPostReplyDto dto);

        Task ForumCommentReply(ForumCommentReplyDto dto);
    }
}
