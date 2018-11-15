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

namespace LesoftWuye2.Core.Mall.Orders
{
    [Table("OrderShips")]
    [EntityDescription("订单发货信息")]
    public class OrderShip : CreationAuditedEntity<long>
    {
        public const int MaxExpressCodeLength = 20;
        public const int MaxExpressNoLength = 100;


        public long OrderId { get; set; }


        [ForeignKey("OrderId")]

        public virtual Order Order { get; set; }


        [MaxLength(MaxExpressCodeLength)]
        public virtual string ExpressCode { get; set; }
        
        [MaxLength(MaxExpressNoLength)]
        public virtual string ExpressNo { get; set; }

        public virtual DateTime? ReceivedTime { get; set; }


        public virtual OrderReceivedType ReceivedType { get; set; }
    }
}
