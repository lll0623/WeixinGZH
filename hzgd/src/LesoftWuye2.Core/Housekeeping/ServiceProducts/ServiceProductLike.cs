using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Products
{
    [Table("ServiceProductLikes")]
    [EntityDescription("家政服务收藏")]
    public class ServiceProductLike : CreationAuditedEntity<long>
    {
        
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public long MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }


    }
}
