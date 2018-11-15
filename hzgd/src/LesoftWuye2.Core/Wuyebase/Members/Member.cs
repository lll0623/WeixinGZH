using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Wuyebase.Members
{
    [Table("Members")]
    [EntityDescription("会员")]
    public class Member : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxThumbnailLength = 1024;
        public const int MaxOpenidLength = 256;
        public const int MaxMemberIdLength = 20;

        [Display(Name = "名称")]
        //[Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }


        [Display(Name = "Openid")]
        [MaxLength(MaxOpenidLength)]

        public virtual string Openid { get; set; }

        [Display(Name = "会员id")]
        [Required]
        [MaxLength(MaxMemberIdLength)]
        public virtual string MemberId { get; set; }

        [Display(Name = "绑定房号信息")]
        public virtual string BindRooms { get; set; }

        public virtual string ThumbnailBase64 { get; set; }

        public virtual string PRoomFullName { get; set; }

        public virtual string ProjectCode { get; set; }

    }
}
