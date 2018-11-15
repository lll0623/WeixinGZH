using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Substations
{
    [Table("Substations")]
    [EntityDescription("祥源股份")]
    public class Substation : AuditedEntity<long>
    {
        public const int MaxNameLength = 20;
        public const int MaxUrlLength = 128;
        public const int MaxThumbnailLength = 128;


        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }


        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "链接")]
        [Required]
        [MaxLength(MaxUrlLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Url { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }
    }
}
