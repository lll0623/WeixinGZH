using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;
using LesoftWuye2.Core.Mall.RefundOrders;

namespace LesoftWuye2.Core.RefundOrders
{
    [Table("RefundOrderImages")]
    [EntityDescription("退款图片")]
    public class RefundOrderImage : AuditedEntity<long>
    {

        public const int MaxThumbnailLength = 128;

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        public long RefundOrderId { get; set; }

        [ForeignKey("RefundOrderId")]

        public virtual RefundOrder RefundOrder { get; set; }
    }
}
