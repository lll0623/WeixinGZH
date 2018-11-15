using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using LesoftWuye2.Application.ForumPosts.Dto;
using LesoftWuye2.Application.Forums.Dto;
using LesoftWuye2.Application.Plates;
using LesoftWuye2.Core.ForumComments;
using LesoftWuye2.Core.ForumImages;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.SqlExecuters;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.Dto;
using Obs.Filter;

namespace LesoftWuye2.Application.ForumPosts
{
    public class ForumPostService : LesoftWuye2AppServiceBase, IForumPostService
    {


        private readonly IPlateAppService _plateAppService;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IRepository<Plate, long> _plateRepository;
        private readonly IRepository<ForumPost, long> _forumPostRepository;
        private readonly IRepository<ForumComment, long> _forumCommentRepository;
        private readonly IRepository<ForumImage, long> _forumImageRepository;
        private readonly ISqlExecuter _sqlExecuter;

        public ForumPostService(
            IPlateAppService plateAppService,
            IRepository<Member, long> memberRepository,
             IRepository<Plate, long> plateRepository,
             IRepository<ForumPost, long> forumPostRepository,
             IRepository<ForumComment, long> forumCommentRepository,
             IRepository<ForumImage, long> forumImageRepository,
             ISqlExecuter sqlExecuter
          )
        {
            _plateAppService = plateAppService;
            _memberRepository = memberRepository;
            _plateRepository = plateRepository;
            _forumPostRepository = forumPostRepository;
            _forumCommentRepository = forumCommentRepository;
            _forumImageRepository = forumImageRepository;
            _sqlExecuter = sqlExecuter;
        }



        public async Task<PageListResultDto<ForumPostListDto>> GetForumPosts(GetPageListRequstDto dto)
        {
            var query = _forumPostRepository.GetAll();
            var where = FilterExpression.FindByGroup<ForumPost>(dto.Filter);
            var queryCount = query.Where(where)
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left,
                        Member = right
                    })
                .GroupJoin(_plateRepository.GetAll(), left => left.Posts.PlateId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left.Posts,
                        Member = left.Member,
                        Plate = right
                    });


            int count = await queryCount.CountAsync();

            var queryList = query.Where(where)
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left,
                        Member = right
                    })
                .GroupJoin(_plateRepository.GetAll(), left => left.Posts.PlateId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left.Posts,
                        Member = left.Member,
                        Plate = right
                    });

            var list = await queryList.OrderByDescending(t => t.Posts.LastCommentTime).PageBy(dto).ToListAsync();


            var posts = list.Select(item => new ForumPostListDto()
            {
                Id = item.Posts.Id,
                CommentCount = item.Posts.CommentCount,
                CreationTime = item.Posts.CreationTime,
                LastCommentTime = item.Posts.LastCommentTime,
                MemberName = item.Member.FirstOrDefault()?.Name,
                PlateName = item.Plate.FirstOrDefault()?.Name,
                ReadCount = item.Posts.ReadCount,
                Title = item.Posts.Title
            }).ToList();


            return new PageListResultDto<ForumPostListDto>(count, posts, dto.CurrentPage, dto.PageSize);
        }

        public void DeleteForumPost(long id)
        {
            //删除帖子所有相关数据
            //图片,回复,帖子本身

            _sqlExecuter.Execute("delete from ForumImages where Type=0 and OwnerId=" + id);

            _sqlExecuter.Execute(
                $"delete from ForumImages where Type=1 and exists (select 1 from ForumComments where ForumImages.OwnerId=ForumComments.Id and ForumComments.ForumPostId={id})");
            _sqlExecuter.Execute("delete from ForumComments where ForumPostId=" + id);
            _sqlExecuter.Execute("delete from ForumPosts where id=" + id);

        }

        public async Task<ForumPostItemDto> GetForumPost(long id)
        {
            var data = await _forumPostRepository.GetAll()
                .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left,
                        Member = right
                    })
                .GroupJoin(_plateRepository.GetAll(), left => left.Posts.PlateId, right => right.Id,
                    (left, right) => new
                    {
                        Posts = left.Posts,
                        Member = left.Member,
                        Plate = right
                    })
                .FirstOrDefaultAsync(t => t.Posts.Id == id);


            ForumPostItemDto detail = new ForumPostItemDto();
            detail.Images = new List<string>();
            detail.Comments = new List<CommentListItem>();
            detail.Id = data.Posts.Id;
            detail.CommentCount = data.Posts.CommentCount;
            detail.Content = data.Posts.Content;
            detail.CreationTime = data.Posts.CreationTime;
            detail.MemberBindRooms = data.Member.FirstOrDefault()?.BindRooms;
            detail.MemberName = data.Member.FirstOrDefault()?.Name;
            detail.MemberThumbnail = data.Member.FirstOrDefault()?.Thumbnail;
            detail.PlateName = data.Plate.FirstOrDefault()?.Name;
            detail.ReadCount = data.Posts.ReadCount;
            detail.Title = data.Posts.Title;
            detail.AdminReply = data.Posts.AdminReply;
            detail.MemberPRoomFullName = data.Member.FirstOrDefault()?.PRoomFullName;

            var iamges =
                    await
                        _forumImageRepository.GetAll()
                            .Where(t => t.OwnerId == detail.Id && t.Type == ForumImageType.Posts)
                            .OrderBy(t => t.Id)
                            .ToListAsync();
            foreach (var iamge in iamges)
            {
                detail.Images.Add(iamge.Image);
            }



            var comments = await _forumCommentRepository.GetAll()
            .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                (left, right) => new
                {
                    PostComment = left,
                    Member = right
                })
            .OrderByDescending(t => t.PostComment.CreationTime)
            .Where(t => t.PostComment.ForumPostId == id)
            .ToListAsync();

            foreach (var comment in comments)
            {
                detail.Comments.Add(new CommentListItem()
                {
                    Id = comment.PostComment.Id,
                    Images = new List<string>(),
                    Content = comment.PostComment.Content,
                    CreationTime = comment.PostComment.CreationTime,
                    Floor = comment.PostComment.Floor,
                    MemberName = comment.Member.FirstOrDefault()?.Name,
                    MemberBindRooms = comment.Member.FirstOrDefault()?.BindRooms,
                    MemberThumbnail = comment.Member.FirstOrDefault()?.Thumbnail,
                    AdminReply=comment.PostComment.AdminReply
                });
            }

            foreach (var comment in detail.Comments)
            {
                var iamges1 =
                   await
                       _forumImageRepository.GetAll()
                           .Where(t => t.OwnerId == comment.Id && t.Type == ForumImageType.Comment)
                           .OrderBy(t => t.Id)
                           .ToListAsync();
                foreach (var iamge in iamges1)
                {
                    comment.Images.Add(iamge.Image);
                }
            }


          

            return detail;
        }

        public void DeleteForumComment(long id)
        {
            _sqlExecuter.Execute("delete from ForumImages where Type=1 and OwnerId=" + id);
            _sqlExecuter.Execute("delete from ForumComments where Id=" + id);
        }

        public async Task ForumPostReply(ForumPostReplyDto dto)
        {
            var postInfo = await _forumPostRepository.FirstOrDefaultAsync(t => t.Id == dto.PostId);
            if (postInfo == null)
                throw new UserFriendlyException("帖子不存在!");

            postInfo.AdminReply = dto.Content;
            postInfo.AdminReplyTime=DateTime.Now;
            await _forumPostRepository.UpdateAsync(postInfo);
        }

        public async Task ForumCommentReply(ForumCommentReplyDto dto)
        {
            var postCommentInfo = await _forumCommentRepository.FirstOrDefaultAsync(t => t.Id == dto.PostCommentId);
            if (postCommentInfo == null)
                throw new UserFriendlyException("帖子评论不存在!");

            postCommentInfo.AdminReply = dto.Content;
            postCommentInfo.AdminReplyTime = DateTime.Now;
            await _forumCommentRepository.UpdateAsync(postCommentInfo);
        }
    }
}
