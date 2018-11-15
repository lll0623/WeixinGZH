using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Core.Estateinfos
{
    [Table("EstateinfoImages")]
    public class EstateinfoImage : CreationAuditedEntity<long>
    {
        public long EstateinfoId { get; set; }

        [ForeignKey("EstateinfoId")]

        public virtual Estateinfo Estateinfo { get; set; }

        [Required]
        public virtual string Image { get; set; }
    }
}
