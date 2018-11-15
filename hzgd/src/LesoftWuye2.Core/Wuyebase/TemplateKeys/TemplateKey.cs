using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.TemplateKeys
{
    [Table("TemplateKeys")]
    [EntityDescription("模板消息key")]
    public class TemplateKey : CreationAuditedEntity<long>
    {
        [Required]
        [MaxLength(128)]
        public virtual string TempMsgKey { get; set; }

        [MaxLength(128)]
        public virtual string TempMsgId { get; set; }

    }
}
