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
using LesoftWuye2.Core.Suppliers;

namespace LesoftWuye2.Core.Products
{
    [Table("Products")]
    [EntityDescription("商品")]
    public class Product : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxSpecificationseLength = 50;
        public const int MaxUnitLength = 20;
        public const int MaxThumbnailLength = 128;
        public const int MaxServiceTagsLength = 100;
        public const int MaxSummaryLength = 100;
        public const int MaxSlideImagesLength = 1024;


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

        [Display(Name = "销量")]
        public virtual int SellCount { get; set; }

        [Display(Name = "是否销售中")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual bool IsSale { get; set; }

        [Display(Name = "价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal Price { get; set; }

        [Display(Name = "销售价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal SalePrice { get; set; }

        [Display(Name = "业主价格")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual decimal MemberPrice { get; set; }

        [Display(Name = "服务标签")]
        [MaxLength(MaxServiceTagsLength)]
        [DtoAssign(DtoAssignTargets.Create| DtoAssignTargets.Update)]
        public virtual string ServiceTags { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }


        [Display(Name = "轮播图片")]
        [MaxLength(MaxSlideImagesLength)]
        [DtoAssign(DtoAssignTargets.Create | DtoAssignTargets.Update)]
        public virtual string SlideImages { get; set; }


        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

        [Display(Name = "商品简介")]
        [MaxLength(MaxSummaryLength)]
        [DtoAssign(DtoAssignTargets.Create | DtoAssignTargets.Update)]
        public virtual string Summary { get; set; }


        [Display(Name = "商品详情")]
        [Required]
        [DtoAssign(DtoAssignTargets.Create | DtoAssignTargets.Update)]
        public virtual string Content { get; set; }

        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]

        public virtual Category Category { get; set; }


        public long SupplierId { get; set; }

        [ForeignKey("SupplierId")]

        public virtual Supplier Supplier { get; set; }
    }
}
