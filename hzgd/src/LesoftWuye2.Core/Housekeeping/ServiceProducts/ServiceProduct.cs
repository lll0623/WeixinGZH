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
using LesoftWuye2.Core.ServiceCategories;

namespace LesoftWuye2.Core.Products
{
    [Table("ServiceProducts")]
    [EntityDescription("家政服务产品")]
    public class ServiceProduct : AuditedEntity<long>
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

        [Display(Name = "团购开始时间")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual DateTime StartTime { get; set; }

        [Display(Name = "团购结束时间")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual DateTime ExpireTime { get; set; }

        public long ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]

        public virtual ServiceCategory ServiceCategory { get; set; }


        public long ServiceSupplierId { get; set; }

        [ForeignKey("ServiceSupplierId")]

        public virtual ServiceSupplier ServiceSupplier { get; set; }
    }
}
