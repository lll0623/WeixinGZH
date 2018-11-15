using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Application.ForumPosts.Dto
{
    public class ForumPostListDto
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string MemberName { get; set; }
        
        public virtual string PlateName { get; set; }
         
        public virtual int ReadCount { get; set; }

        public virtual int CommentCount { get; set; }

        public virtual DateTime LastCommentTime { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
