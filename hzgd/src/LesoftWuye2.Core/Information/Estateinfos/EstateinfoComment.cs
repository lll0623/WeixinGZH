using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace LesoftWuye2.Core.Estateinfos
{
    [Table("EstateinfoComments")]
    public class EstateinfoComment : CreationAuditedEntity<long>
    {
        public const int MaxContentLength = 500;
        public const int MaxMemberNameLength = 20;


        public long EstateinfoId { get; set; }

        [ForeignKey("EstateinfoId")]
        public virtual Estateinfo Estateinfo { get; set; }
        
        [Display(Name = "发布人")]
        [Required] 
        public virtual string MemberId { get; set; }

        [Required]
        [Display(Name = "发布人")]
        [MaxLength(MaxMemberNameLength)]
        public virtual string MemberName { get; set; }

        [Required]
        [Display(Name = "内容")]
        [MaxLength(MaxContentLength)]
        public virtual string Content { get; set; }

    }
}
