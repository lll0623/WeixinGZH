using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ServiceTels
{
    [Table("ServiceTels")]
    [EntityDescription("服务电话")]
    public class ServiceTel : AuditedEntity<long>
    {
        public const int MaxNameLength = 20;
        public const int MaxTelLength = 20;

        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "电话号码")]
        [Required]
        [MaxLength(MaxTelLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Tel { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }
    }
}
