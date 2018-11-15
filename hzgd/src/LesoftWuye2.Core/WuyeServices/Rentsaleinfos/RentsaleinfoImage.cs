using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Projects;

namespace LesoftWuye2.Core.Rentsaleinfos
{
    [Table("RentsaleinfoImages")]
    public class RentsaleinfoImage : CreationAuditedEntity<long>
    {
        public long RentsaleinfoId { get; set; }

        [ForeignKey("RentsaleinfoId")]

        public virtual Rentsaleinfo Rentsaleinfo { get; set; }

        [Required]
        public virtual string Image { get; set; }
    }
}
