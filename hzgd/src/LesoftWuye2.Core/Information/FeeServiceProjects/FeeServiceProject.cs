using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.FeeServices;
using LesoftWuye2.Core.Notices;
using LesoftWuye2.Core.Projects;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.FeeServiceProjects
{
    [Table("FeeServiceProjects")]
    [EntityDescription("有偿服务所属小区")]
    public class FeeServiceProject : CreationAuditedEntity<long>
    {
        public virtual long FeeServiceId { get; set; }
      
        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]
       
        public virtual Project Project { get; set; }

        [ForeignKey("FeeServiceId")]
        public virtual FeeService FeeService { get; set; }
    }
}
