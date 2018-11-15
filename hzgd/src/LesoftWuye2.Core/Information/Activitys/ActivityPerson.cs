using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Activitys
{
    [Table("ActivityPersons")]
    [EntityDescription("活动参加人员")]
    public class ActivityPerson : CreationAuditedEntity<long>
    {
        public const int MaxContactLength = 50;
        public const int MaxMobileLength = 128;

        public long ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }

        [Display(Name = "发布人")]
        [Required]
        public virtual string MemberId { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(MaxMobileLength)]
        public virtual string Mobile { get; set; }
    }
}
