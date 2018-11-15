using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.WeixinSubscribes
{
    [Table("WeixinSubscribes")]
    [EntityDescription("微信关注消息")]
    public class WeixinSubscribe : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxThumbnailLength = 128;
        public const int MaxSummarylLength = 500;
        public const int MaxUrlLength = 128;


        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxTitleLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Title { get; set; }

        [Display(Name = "简介")]
        [Required]
        [MaxLength(MaxSummarylLength)]
        public virtual string Summary { get; set; }

        [Display(Name = "缩略图")]
        [Required]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "链接")]
        [MaxLength(MaxUrlLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Url { get; set; }


        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }
    }
}
