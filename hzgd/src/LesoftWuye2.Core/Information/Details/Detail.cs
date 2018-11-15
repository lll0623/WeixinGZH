using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Details
{
    [Table("Details")]
    [EntityDescription("内容详情")]
    public class Detail : AuditedEntity<long>
    {
        [Column(Order = 0)]
        public DetailType Type { get; set; }

        [Column(Order = 1)]
        public virtual long DataId { get; set; }

        public virtual string Content { get; set; }
    }
}
