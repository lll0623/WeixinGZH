using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Suppliers;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Core.Orders
{
    [Table("GrouponMembers")]
    [EntityDescription("团购成员")]
    public class GrouponMember : AuditedEntity<long>
    {
        public const int MaxChannelLength = 20;
        public const int MaxTradeNoLength = 256;

        public long MemberId { get; set; }

        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }

        public virtual bool IsHeader { get; set; }

        public long GrouponOrderId { get; set; }

        [ForeignKey("GrouponOrderId")]

        public virtual GrouponOrder GrouponOrder { get; set; }


        #region 退款信息

        //[Required]
        [MaxLength(MaxChannelLength)]
        public virtual string Channel { get; set; }

        //[Required]
        [MaxLength(MaxTradeNoLength)]
        public virtual string TradeNo { get; set; }

        public virtual decimal Money { get; set; }

        public virtual DateTime? RefundsTime { get; set; } 

        #endregion

    }
}
