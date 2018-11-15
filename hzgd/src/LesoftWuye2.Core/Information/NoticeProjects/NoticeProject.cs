using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.NoticeProjects
{
    [Table("NoticeProjects")]
    [EntityDescription("公告所属小区")]
    public class NoticeProject : CreationAuditedEntity<long>
    {
        public virtual long NoticeId { get; set; }
      
        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]
       
        public virtual Project Project { get; set; }

        [ForeignKey("NoticeId")]
        public virtual Notice Notice { get; set; }
    }
}
