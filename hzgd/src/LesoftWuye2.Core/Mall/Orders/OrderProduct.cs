using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Mall.Orders
{
    [Table("OrderProducts")]
    [EntityDescription("订单商品")]
    public class OrderProduct : AuditedEntity<long>
    {


        public const int MaxNameLength = 50;
        public const int MaxSpecificationseLength = 50;
        public const int MaxUnitLength = 20;
        public const int MaxThumbnailLength = 128;
        public const int MaxServiceTagsLength = 100;
        public const int MaxSummaryLength = 100;
        public const int MaxSlideImagesLength = 1024;

        public long OrderId { get; set; }


        [ForeignKey("OrderId")]

        public virtual Order Order { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]

        public virtual Product Product { get; set; }


        [Display(Name = "价格")]
        public virtual decimal GrouponPrice { get; set; }


        [Display(Name = "名称")]
        [Required]
        [MaxLength(MaxNameLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Name { get; set; }

        [Display(Name = "规格")]
        [Required]
        [MaxLength(MaxSpecificationseLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Specification { get; set; }

        [Display(Name = "单位")]
        [Required]
        [MaxLength(MaxUnitLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Unit { get; set; }

        [Display(Name = "价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal Price { get; set; }

        [Display(Name = "销售价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal SalePrice { get; set; }

        [Display(Name = "业主价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal MemberPrice { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "商品简介")]
        [MaxLength(MaxSummaryLength)]
        [DtoAssign(DtoAssignTargets.Create | DtoAssignTargets.Update)]
        public virtual string Summary { get; set; }

        public virtual int Num { get; set; }

    }
}
