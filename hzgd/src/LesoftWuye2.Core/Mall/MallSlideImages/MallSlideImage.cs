using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.MallSlideImages
{
    [Table("MallSlideImages")]
    [EntityDescription("轮播图")]
    public class MallSlideImage : AuditedEntity<long>
    {

        public const int MaxThumbnailLength = 128;
        public const int MaxUrlLength = 256;

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "链接")]
        [MaxLength(MaxUrlLength)]
        public virtual string Url { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }
    }
}
