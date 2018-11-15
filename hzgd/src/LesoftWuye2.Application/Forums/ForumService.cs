using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Components.DictionaryAdapter;
using LesoftWuye2.Application.Forums.Dto;
using LesoftWuye2.Application.Groupons;
using LesoftWuye2.Application.Malls.Dto;
using LesoftWuye2.Application.Plates;
using LesoftWuye2.Application.Plates.Dto;
using LesoftWuye2.Core.ForumComments;
using LesoftWuye2.Core.ForumImages;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.Dto;
using Obs.Filter;

namespace LesoftWuye2.Application.Forums
{
    public class ForumService : LesoftWuye2AppServiceBase, IForumService
    {
        private readonly IPlateAppService _plateAppService;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IRepository<Plate, long> _plateRepository;
        private readonly IRepository<ForumPost, long> _forumPostRepository;
        private readonly IRepository<ForumComment, long> _forumCommentRepository;
        private readonly IRepository<ForumImage, long> _forumImageRepository;


        public ForumService(
            IPlateAppService plateAppService,
            IRepository<Member, long> memberRepository,
             IRepository<Plate, long> plateRepository,
             IRepository<ForumPost, long> forumPostRepository,
             IRepository<ForumComment, long> forumCommentRepository,
             IRepository<ForumImage, long> forumImageRepository
          )
        {
            _plateAppService = plateAppService;
            _memberRepository = memberRepository;
            _plateRepository = plateRepository;
            _forumPostRepository = forumPostRepository;
            _forumCommentRepository = forumCommentRepository;
            _forumImageRepository = forumImageRepository;
        }

        public async Task<ForumHomeData> GetHomeData()
        {
            ForumHomeData data = new ForumHomeData();
            data.Plates = (await _plateAppService.GetPlates(new GetPageListRequstDto())).Items;

            var list = await _forumPostRepository.GetAll()
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
                 .OrderByDescending(t => t.Posts.LastCommentTime)
                 .Take(5)
                 .ToListAsync();

            data.TopPosts = new EditableList<PostListItem>();
            foreach (var item in list)
            {
                data.TopPosts.Add(new PostListItem()
                {
                    Id = item.Posts.Id,
                    CommentCount = item.Posts.CommentCount,
                    CreationTime = item.Posts.CreationTime,
                    MemberBindRooms = item.Member.FirstOrDefault()?.BindRooms,
                    MemberName = item.Member.FirstOrDefault()?.Name,
                    MemberThumbnail = item.Member.FirstOrDefault()?.Thumbnail,
                    Images = new List<string>(),
                    PlateName = item.Plate.FirstOrDefault()?.Name,
                    ReadCount = item.Posts.ReadCount,
                    Title = item.Posts.Title,
                    Summary = item.Posts.Summary
                });
            }

            foreach (var post in data.TopPosts)
            {
                var iamges =
                    await
                        _forumImageRepository.GetAll()
                            .Where(t => t.OwnerId == post.Id && t.Type == ForumImageType.Posts)
                            .OrderBy(t => t.Id)
                            .Take(3)
                            .ToListAsync();
                foreach (var iamge in iamges)
                {
                    post.Images.Add(iamge.Image);
                }
            }

            return data;
        }

        public async Task NewPost(NewPostDto dto)
        {
            var memberInfo = await _memberRepository.FirstOrDefaultAsync(t => t.Id == dto.MemberId);
            if (memberInfo == null)
            {
                throw new UserFriendlyException("请先登录账户!");
            }
            var plateInfo = await _plateRepository.FirstOrDefaultAsync(t => t.Id == dto.PlateId);
            if (plateInfo == null)
            {
                throw new UserFriendlyException("请选择发帖版块!");
            }
            if (dto.Title.Length < 5)
            {
                throw new UserFriendlyException("标题不能少于5个字");
            }
            if (dto.Content.Length < 5)
            {
                throw new UserFriendlyException("帖子内容不能少于5个字");
            }

            ForumPost forumPost = new ForumPost();
            forumPost.MemberId = dto.MemberId;
            forumPost.PlateId = dto.PlateId;
            forumPost.Title = dto.Title;
            forumPost.Content = dto.Content;
            if (dto.Content.Length <= 100)
                forumPost.Summary = dto.Content;
            else
            {
                forumPost.Summary = dto.Content.Substring(0, 100);
            }
            forumPost.LastCommentTime = DateTime.Now;

            var postId = await _forumPostRepository.InsertAndGetIdAsync(forumPost);

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var image in dto.Images)
                {
                    ForumImage forumImage = new ForumImage();
                    forumImage.Type = ForumImageType.Posts;
                    forumImage.Image = image;
                    forumImage.OwnerId = postId;
                    await _forumImageRepository.InsertAsync(forumImage);
                }
            }
        }

        public async Task<PostDetail> Detail(long id)
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


            PostDetail detail = new PostDetail();
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
            .Take(10)
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


            data.Posts.ReadCount++;
            await _forumPostRepository.UpdateAsync(data.Posts);

            return detail;
        }

        public async Task NewComment(NewCommentDto dto)
        {
            var memberInfo = await _memberRepository.FirstOrDefaultAsync(t => t.Id == dto.MemberId);
            if (memberInfo == null)
            {
                throw new UserFriendlyException("请先登录账户!");
            }
            var postInfo = await _forumPostRepository.FirstOrDefaultAsync(t => t.Id == dto.PostId);
            if (postInfo == null)
            {
                throw new UserFriendlyException("回复的帖子不存在!");
            }
            if (dto.Content.Length < 5)
            {
                throw new UserFriendlyException("回复内容不能少于5个字");
            }

            if (dto.Content.Length > 100)
            {
                throw new UserFriendlyException("回复内容不能超过100个字");
            }

            ForumComment forumComment = new ForumComment();
            forumComment.MemberId = dto.MemberId;
            forumComment.ForumPostId = postInfo.Id;
            forumComment.Content = dto.Content;
            forumComment.Floor = postInfo.CommentCount + 1;

            postInfo.LastCommentTime = DateTime.Now;
            postInfo.CommentCount++;

            var commentId = await _forumCommentRepository.InsertAndGetIdAsync(forumComment);

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var image in dto.Images)
                {
                    ForumImage forumImage = new ForumImage();
                    forumImage.Type = ForumImageType.Comment;
                    forumImage.Image = image;
                    forumImage.OwnerId = commentId;
                    await _forumImageRepository.InsertAsync(forumImage);
                }
            }

            await _forumPostRepository.UpdateAsync(postInfo);
        }

        public async Task<PageListResultDto<PostListItem>> GetPostList(int plateId, GetPageListRequstDto dto)
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


            queryCount = queryCount.Where(t => t.Posts.PlateId == plateId);

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
            queryList = queryList.Where(t => t.Posts.PlateId == plateId);
            var list = await queryList.OrderByDescending(t => t.Posts.LastCommentTime).PageBy(dto).ToListAsync();


            var posts = list.Select(item => new PostListItem()
            {
                Id = item.Posts.Id,
                CommentCount = item.Posts.CommentCount,
                CreationTime = item.Posts.CreationTime,
                MemberBindRooms = item.Member.FirstOrDefault()?.BindRooms,
                MemberName = item.Member.FirstOrDefault()?.Name,
                MemberThumbnail = item.Member.FirstOrDefault()?.Thumbnail,
                Images = new List<string>(),
                PlateName = item.Plate.FirstOrDefault()?.Name,
                ReadCount = item.Posts.ReadCount,
                Title = item.Posts.Title,
                Summary = item.Posts.Summary
            }).ToList();

            foreach (var post in posts)
            {
                var iamges =
                    await
                        _forumImageRepository.GetAll()
                            .Where(t => t.OwnerId == post.Id && t.Type == ForumImageType.Posts)
                            .OrderBy(t => t.Id)
                            .Take(3)
                            .ToListAsync();
                foreach (var iamge in iamges)
                {
                    post.Images.Add(iamge.Image);
                }
            }

            return new PageListResultDto<PostListItem>(count, posts, dto.CurrentPage, dto.PageSize);
        }

        public async Task<IReadOnlyList<PlateListDto>> GetPlates()
        {
            return (await _plateAppService.GetPlates(new GetPageListRequstDto())).Items;

        }

        public async Task<PageListResultDto<CommentListItem>> GetCommentList(int postId, GetPageListRequstDto dto)
        {
            var query = _forumCommentRepository.GetAll();
            var where = FilterExpression.FindByGroup<ForumComment>(dto.Filter);
            var queryCount = query.Where(where)
            .GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                (left, right) => new
                {
                    PostComment = left,
                    Member = right
                });

            queryCount = queryCount.Where(t => t.PostComment.ForumPostId == postId);
            int count = await queryCount.CountAsync();



            var queryList = query.Where(where).GroupJoin(_memberRepository.GetAll(), left => left.MemberId, right => right.Id,
                (left, right) => new
                {
                    PostComment = left,
                    Member = right
                });

            queryList = queryList.Where(t => t.PostComment.ForumPostId == postId);
            var list = await queryList.OrderByDescending(t => t.PostComment.CreationTime).PageBy(dto).ToListAsync();
            List<CommentListItem> comments = new List<CommentListItem>();

            foreach (var comment in list)
            {
                comments.Add(new CommentListItem()
                {
                    Id = comment.PostComment.Id,
                    Images = new List<string>(),
                    Content = comment.PostComment.Content,
                    CreationTime = comment.PostComment.CreationTime,
                    Floor = comment.PostComment.Floor,
                    MemberName = comment.Member.FirstOrDefault()?.Name,
                    MemberBindRooms = comment.Member.FirstOrDefault()?.BindRooms,
                    MemberThumbnail = comment.Member.FirstOrDefault()?.Thumbnail
                });
            }

            foreach (var comment in comments)
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


            return new PageListResultDto<CommentListItem>(count, comments, dto.CurrentPage, dto.PageSize);
        }

        public async Task<PageListResultDto<PostListItem>> GetMyPostList(long memberId, GetPageListRequstDto dto)
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


            queryCount = queryCount.Where(t => t.Posts.MemberId == memberId);

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
            queryList = queryList.Where(t => t.Posts.MemberId == memberId);
            var list = await queryList.OrderByDescending(t => t.Posts.LastCommentTime).PageBy(dto).ToListAsync();


            var posts = list.Select(item => new PostListItem()
            {
                Id = item.Posts.Id,
                CommentCount = item.Posts.CommentCount,
                CreationTime = item.Posts.CreationTime,
                MemberBindRooms = item.Member.FirstOrDefault()?.BindRooms,
                MemberName = item.Member.FirstOrDefault()?.Name,
                MemberThumbnail = item.Member.FirstOrDefault()?.Thumbnail,
                Images = new List<string>(),
                PlateName = item.Plate.FirstOrDefault()?.Name,
                ReadCount = item.Posts.ReadCount,
                Title = item.Posts.Title,
                Summary = item.Posts.Summary
            }).ToList();

            foreach (var post in posts)
            {
                var iamges =
                    await
                        _forumImageRepository.GetAll()
                            .Where(t => t.OwnerId == post.Id && t.Type == ForumImageType.Posts)
                            .OrderBy(t => t.Id)
                            .Take(3)
                            .ToListAsync();
                foreach (var iamge in iamges)
                {
                    post.Images.Add(iamge.Image);
                }
            }

            return new PageListResultDto<PostListItem>(count, posts, dto.CurrentPage, dto.PageSize);
        }

        public async Task<PageListResultDto<PostListItem>> GetMyCommentList(long memberId, GetPageListRequstDto dto)
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
                    })
                .Join(_forumCommentRepository.GetAll(), left => left.Posts.Id, right => right.ForumPostId,
                    (left, right) => new
                    {
                        Posts = left.Posts,
                        Member = left.Member,
                        Plate = left.Plate,
                        Comments = right
                    });

            queryCount = queryCount.Where(t => t.Comments.MemberId == memberId);

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
                    }).Join(_forumCommentRepository.GetAll(), left => left.Posts.Id, right => right.ForumPostId,
                    (left, right) => new
                    {
                        Posts = left.Posts,
                        Member = left.Member,
                        Plate = left.Plate,
                        Comments = right
                    });
            queryList = queryList.Where(t => t.Comments.MemberId == memberId);
            var list = await queryList.OrderByDescending(t => t.Posts.LastCommentTime).PageBy(dto).ToListAsync();

            var posts=new List<PostListItem>();
            foreach (var item in list)
            {
                if(posts.Any(t=>t.Id==item.Posts.Id))
                    continue;

                posts.Add(new PostListItem()
                {
                    Id = item.Posts.Id,
                    CommentCount = item.Posts.CommentCount,
                    CreationTime = item.Posts.CreationTime,
                    MemberBindRooms = item.Member.FirstOrDefault()?.BindRooms,
                    MemberName = item.Member.FirstOrDefault()?.Name,
                    MemberThumbnail = item.Member.FirstOrDefault()?.Thumbnail,
                    Images = new List<string>(),
                    PlateName = item.Plate.FirstOrDefault()?.Name,
                    ReadCount = item.Posts.ReadCount,
                    Title = item.Posts.Title,
                    Summary = item.Posts.Summary
                });
            }

            //var posts = list.Select(item => new PostListItem()
            //{
            //    Id = item.Posts.Id,
            //    CommentCount = item.Posts.CommentCount,
            //    CreationTime = item.Posts.CreationTime,
            //    MemberBindRooms = item.Member.FirstOrDefault()?.BindRooms,
            //    MemberName = item.Member.FirstOrDefault()?.Name,
            //    MemberThumbnail = item.Member.FirstOrDefault()?.Thumbnail,
            //    Images = new List<string>(),
            //    PlateName = item.Plate.FirstOrDefault()?.Name,
            //    ReadCount = item.Posts.ReadCount,
            //    Title = item.Posts.Title,
            //    Summary = item.Posts.Summary
            //}).ToList();

            foreach (var post in posts)
            {
                var iamges =
                    await
                        _forumImageRepository.GetAll()
                            .Where(t => t.OwnerId == post.Id && t.Type == ForumImageType.Posts)
                            .OrderBy(t => t.Id)
                            .Take(3)
                            .ToListAsync();
                foreach (var iamge in iamges)
                {
                    post.Images.Add(iamge.Image);
                }
            }

            return new PageListResultDto<PostListItem>(count, posts, dto.CurrentPage, dto.PageSize);
        }
    }
}
