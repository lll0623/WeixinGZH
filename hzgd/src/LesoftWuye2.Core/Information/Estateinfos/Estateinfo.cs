using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.EstateinfoTypes;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Estateinfos
{
    [Table("Estateinfos")]
    [EntityDescription("跳蚤信息")]
    public class Estateinfo : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxThumbnailLength = 128;
        public const int MaxSummaryLength = 100;
        public const int MaxContactLength = 10;
        public const int MaxMobileLength = 11;

        [Display(Name = "发布人")]
        [Required]
        public virtual string MemberId { get; set; }


        [DtoAssign(DtoAssignTargets.All)]
        public long EstateinfoTypeId { get; set; }

        [ForeignKey("EstateinfoTypeId")]
        public virtual EstateinfoType Type { get; set; }


        [Display(Name = "标题")]
        [Required]
        [MaxLength(MaxTitleLength)]
        public virtual string Title { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "简介")]
        [MaxLength(MaxSummaryLength)]
        public virtual string Summary { get; set; }

        //[Required]
        //[Display(Name = "内容")]
        //public virtual string Content { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(MaxMobileLength)]
        public virtual string Mobile { get; set; }

        [Display(Name = "是否显示")]
        public virtual bool IsShow { get; set; }

        [Display(Name = "是否上架")]
        public virtual bool IsSale { get; set; }

    }
}
