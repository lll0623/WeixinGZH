using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.FeeServices
{
    [Table("FeeServices")]
    [EntityDescription("有偿服务")]
    public class FeeService : AuditedEntity<long>
    {
        public const int MaxTitleLength = 50;
        public const int MaxThumbnailLength = 128;

        [Display(Name = "标题")]
        [Required]
        [MaxLength(MaxTitleLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Title { get; set; }
        
        [Display(Name = "不区分小区")]
        public bool AllProjects { get; set; }
        
    }
}
