using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Application.Forums.Dto;

namespace LesoftWuye2.Application.ForumPosts.Dto
{
    public class ForumPostItemDto
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string MemberThumbnail { get; set; }
        public string MemberBindRooms { get; set; }

        public virtual string MemberPRoomFullName { get; set; }

        public string PlateName { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }

        public string Content { get; set; }

        public List<string> Images { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual int ReadCount { get; set; }

        public virtual int CommentCount { get; set; }

        public List<CommentListItem> Comments { get; set; }

        public virtual string AdminReply { get; set; }
    }
}
