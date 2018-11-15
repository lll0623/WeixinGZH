using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Mall.RefundOrders
{
    [Table("RefundOrders")]
    [EntityDescription("退款单")]
    public class RefundOrder : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxSpecificationseLength = 50;
        public const int MaxUnitLength = 20;
        public const int MaxThumbnailLength = 128;
        public const int MaxServiceTagsLength = 100;
        public const int MaxSummaryLength = 100;
        public const int MaxSlideImagesLength = 1024;
        public const int MaxRemarkLength = 200;

        public long OrderId { get; set; }


        [ForeignKey("OrderId")]

        public virtual Order Order { get; set; }

        public string OrderNo { get; set; }


        public long MemberId { get; set; }


        [ForeignKey("MemberId")]

        public virtual Member Member { get; set; }

        public virtual decimal Amount { get; set; }

        public long ProductId { get; set; }

        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [Display(Name = "规格")]
        [Required]
        [MaxLength(MaxSpecificationseLength)]
        public virtual string Specification { get; set; }

        [Display(Name = "单位")]
        [Required]
        [MaxLength(MaxUnitLength)]
        public virtual string Unit { get; set; }

        [Display(Name = "价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal Price { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        public virtual string Thumbnail { get; set; }

        public virtual int Num { get; set; }

        public RefundType Type { get; set; }

        public RefundReason Reason { get; set; }

        [Display(Name = "退款金额")]
        public decimal RefundMoney { get; set; }

        public string Mobile { get; set; }

        [Display(Name = "退款说明")]
        [MaxLength(MaxRemarkLength)]
        public string Remark { get; set; }

        public RefundStatus Status { get; set; }

    }
}
