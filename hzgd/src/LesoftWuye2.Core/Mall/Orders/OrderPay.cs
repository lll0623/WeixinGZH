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
    [Table("OrderPays")]
    [EntityDescription("订单支付信息")]
    public class OrderPay : AuditedEntity<long>
    {
        public const int MaxChannelLength = 20;
        public const int MaxTradeNoLength = 256;


        public long OrderId { get; set; }


        [ForeignKey("OrderId")]

        public virtual Order Order { get; set; }

        [Required]
        [MaxLength(MaxChannelLength)]
        public virtual string Channel { get; set; }

        [Required]
        [MaxLength(MaxTradeNoLength)]
        public virtual string TradeNo { get; set; }

        public virtual decimal Money { get; set; }

        public virtual DateTime PayTime { get; set; }


        public virtual DateTime? RefundsPayTime { get; set; }
        public virtual string RefundsTradeNo { get; set; }
    }
}
