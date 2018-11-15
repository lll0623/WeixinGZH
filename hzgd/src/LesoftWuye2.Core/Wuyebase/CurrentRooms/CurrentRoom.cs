using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.CurrentRooms
{
    [Table("CurrentRooms")]
    [EntityDescription("服务电话")]
    public class CurrentRoom : AuditedEntity<long>
    {
        public const int MaxNameLength = 100;
        public const int MaxCodeLength = 50;
        public const int MaxUserIdLength = 20;

        [Display(Name = "会员id")]
        [DtoAssign(DtoAssignTargets.All)]
        [MaxLength(MaxUserIdLength)]
        public virtual string UserId { get; set; }

        [Display(Name = "房间名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "房间编号")]
        [Required]
        [MaxLength(MaxCodeLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Code { get; set; }


    }
}
