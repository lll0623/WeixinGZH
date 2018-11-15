using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.ForumPosts;
using LesoftWuye2.Core.Plates;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ForumComments
{
    [Table("ForumComments")]
    [EntityDescription("帖子回复")]
    public class ForumComment : AuditedEntity<long>
    {
        public const int MaxContentLength = 100;


        public const int MaxAdminReplyLength = 500;

        [Display(Name = "回帖内容")]
        [Required]
        [MaxLength(MaxContentLength)]
        public virtual string Content { get; set; }

        public long MemberId { get; set; }

        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }

        public long ForumPostId { get; set; }

        [ForeignKey("ForumPostId")]

        public virtual ForumPost ForumPost { get; set; }

        public virtual int Floor { get; set; }


        [MaxLength(MaxAdminReplyLength)]
        public virtual string AdminReply { get; set; }

        public virtual DateTime? AdminReplyTime { get; set; }
    }
}
