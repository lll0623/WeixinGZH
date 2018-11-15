using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.RefundOrders;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Mall.Orders
{
    [Table("Orders")]
    [EntityDescription("订单")]
    public class Order : AuditedEntity<long>
    {
        public const int MaxAddressLength = 255;
        public const int MaxContactLength = 20;
        public const int MaxMobileLength = 11;
        public const int MaxClientRemarkLength = 255;


        public string OrderNo { get; set; }

        public long MemberId { get; set; }


        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual OrderType Type { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual GrouponStatus GrouponStatus { get; set; }

        /// <summary>
        /// 加入的拼团（下单确定）
        /// </summary>
        public virtual long JoinGrouponId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long GrouponId { get; set; }

        public virtual bool IsPay { get; set; }

        #region 收货地址 

        [Required]
        [MaxLength(MaxAddressLength)]
        public virtual string Address { get; set; }

        [Required]
        [MaxLength(MaxMobileLength)]
        public virtual string Mobile { get; set; }

        [Required]
        [MaxLength(MaxContactLength)]
        public virtual string Contact { get; set; }
        public virtual long ProvinceId { get; set; }
        public virtual long CityId { get; set; }
        public virtual long DistrictId { get; set; }

        #endregion

        [MaxLength(MaxClientRemarkLength)]
        public string ClientRemark { get; set; }


        public RefundStatus RefundStatus { get; set; }
    }
}
