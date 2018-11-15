using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.LifeInfoProjects
{
    [Table("LifeInfoProjects")]
    [EntityDescription("生活信息所属小区")]
    public class LifeInfoProject : CreationAuditedEntity<long>
    {
        public virtual long LifeInfoId { get; set; }

        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]

        public virtual Project Project { get; set; }

        [ForeignKey("LifeInfoId")]
        public virtual LifeInfo LifeInfo { get; set; }
    }
}
