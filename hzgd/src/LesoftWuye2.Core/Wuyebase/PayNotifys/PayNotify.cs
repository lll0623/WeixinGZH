using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.PayNotifys
{
    [Table("PayNotify")]
    [EntityDescription("支付通知")]
    public class PayNotify : CreationAuditedEntity<long>
    {
        public const int MaxSourceLength = 20;
        public const int MaxOrderNoLength = 50;
        public const int MaxTradeNoLength = 50;


        [Display(Name = "来源")]
        [Required]
        [MaxLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        [Display(Name = "订单号")]
        [Required]
        [MaxLength(MaxOrderNoLength)]
        public virtual string OrderNo { get; set; }

        [Display(Name = "支付流水号")]
        [Required]
        [MaxLength(MaxTradeNoLength)]
        public virtual string TradeNo { get; set; }

        [Display(Name = "请求参数")]
        [Required]
        public virtual string Request { get; set; }

        [Display(Name = "金额")]
        public virtual decimal Money { get; set; }

        [Display(Name = "请求接口返回的响应")]
        [Required]
        public virtual string Response { get; set; }

        [Display(Name = "是否处理成功")]
        public virtual bool Success { get; set; }
    }
}
