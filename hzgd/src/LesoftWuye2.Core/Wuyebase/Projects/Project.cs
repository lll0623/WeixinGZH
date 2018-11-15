using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Projects
{
    [Table("Projects")]
    [EntityDescription("小区")]
    public class Project : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxCodeLength = 20;

        [Display(Name = "小区名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "小区代号")]
        [Required]
        [MaxLength(MaxCodeLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Code { get; set; }
    }
}