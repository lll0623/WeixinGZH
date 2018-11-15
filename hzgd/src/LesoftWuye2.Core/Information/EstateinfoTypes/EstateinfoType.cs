using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.EstateinfoTypes
{
    [Table("EstateinfoTypes")]
    [EntityDescription("跳蚤信息分类")]
    public class EstateinfoType : AuditedEntity<long>
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
