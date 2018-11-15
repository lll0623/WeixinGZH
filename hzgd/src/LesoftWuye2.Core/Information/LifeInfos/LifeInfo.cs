using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.LifeInfoTypes;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.LifeInfos
{
    [Table("LifeInfos")]
    [EntityDescription("生活服务")]
    public class LifeInfo : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxSummaryLength = 500;
        public const int MaxThumbnailLength = 128;

        [Display(Name = "标题")]
        [Required]
        [MaxLength(MaxTitleLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Title { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "不区分小区")]
        public bool AllProjects { get; set; }

        [Display(Name = "标题")]
        [MaxLength(MaxSummaryLength)]
        [DtoAssign(DtoAssignTargets.Create|DtoAssignTargets.Update)]
        public virtual string Summary { get; set; }

        [DtoAssign(DtoAssignTargets.All)]
        public long LifeInfoTypeId { get; set; }

        [ForeignKey("LifeInfoTypeId")]
        public virtual LifeInfoType Type { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

    }
}
