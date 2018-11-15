using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.RentsaleinfoTypes
{
    [Table("RentsaleinfoTypes")]
    [EntityDescription("租售信息分类")]
    public class RentsaleinfoType : AuditedEntity<long>
    {
        public const int MaxNameLength = 20;

        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }


        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }
    }
}
