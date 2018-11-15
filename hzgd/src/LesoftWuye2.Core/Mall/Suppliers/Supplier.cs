using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Suppliers
{
    [Table("Suppliers")]
    [EntityDescription("供应商")]
    public class Supplier : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;

        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }
    }
}
