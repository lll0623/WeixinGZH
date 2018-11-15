using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Activitys
{
    [Table("Activitys")]
    [EntityDescription("小区活动")]
    public class Activity : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxThumbnailLength = 128;

        [Display(Name = "活动标题")]
        [Required]
        [MaxLength(MaxTitleLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Title { get; set; }

        [Display(Name = "活动缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "不区分小区")]
        public bool AllProjects { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

        [Display(Name = "报名截止日期")]
        public virtual DateTime Expireday { get; set; }

        [Display(Name = "允许报名人数")]
        public virtual int AllowCount { get; set; }

        [Display(Name = "已报名人数")]
        public virtual int JoinCount { get; set; }
    }
}
