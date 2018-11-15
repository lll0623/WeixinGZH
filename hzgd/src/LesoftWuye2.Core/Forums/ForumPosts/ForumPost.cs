using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ForumPosts
{
    [Table("ForumPosts")]
    [EntityDescription("论坛帖子")]
    public class ForumPost : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxContentLength = 65536;
        public const int MaxSummaryLength = 100;

        public const int MaxAdminReplyLength = 500;



        [Display(Name = "帖子标题")]
        [Required]
        [MaxLength(MaxTitleLength)]
        public virtual string Title { get; set; }

        public long MemberId { get; set; }

        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }

        public long PlateId { get; set; }

        [ForeignKey("PlateId")]

        public virtual Plate Plate { get; set; }

        public virtual int ReadCount { get; set; }

        public virtual int CommentCount { get; set; }

        public virtual DateTime LastCommentTime { get; set; }


        [MaxLength(MaxContentLength)]
        public virtual string Content { get; set; }


        [MaxLength(MaxSummaryLength)]
        public virtual string Summary { get; set; }

        [MaxLength(MaxAdminReplyLength)]
        public virtual string AdminReply { get; set; }

        public virtual DateTime? AdminReplyTime { get; set; }
    }
}
