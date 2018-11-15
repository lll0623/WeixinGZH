using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Newss;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.NewsProjects
{
    [Table("NewsProjects")]
    [EntityDescription("新闻所属小区")]
    public class NewsProject : CreationAuditedEntity<long>
    {
        public virtual long NewsId { get; set; }

        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("NewsId")]
        public virtual News News { get; set; }
    }
}
