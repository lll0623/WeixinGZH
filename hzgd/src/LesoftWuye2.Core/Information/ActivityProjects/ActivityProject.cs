using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Activitys;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.ActivityProjects
{
    [Table("ActivityProjects")]
    [EntityDescription("活动所属小区")]
    public class ActivityProject : CreationAuditedEntity<long>
    {
        public virtual long ActivityId { get; set; }

        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]

        public virtual Project Project { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }
    }
}
