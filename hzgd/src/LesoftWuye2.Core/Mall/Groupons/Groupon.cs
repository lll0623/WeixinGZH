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
using LesoftWuye2.Core.Products;

namespace LesoftWuye2.Core.Groupons
{
    [Table("Groupons")]
    [EntityDescription("团购")]
    public class Groupon : AuditedEntity<long>
    {
        public const int MaxNameLength = 50;
        public const int MaxThumbnailLength = 128;
        public const int MaxSummaryLength = 500;

        [Display(Name = "价格")]
        public virtual decimal Price { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

        [Display(Name = "商品简介")]
        [MaxLength(MaxSummaryLength)]
        [DtoAssign(DtoAssignTargets.Create | DtoAssignTargets.Update)]
        public virtual string Summary { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]

        public virtual Product Product { get; set; }

        [Display(Name = "要求人数")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int RequireCount { get; set; }

        [Display(Name = "是否团购中")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual bool IsSale { get; set; }

        [Display(Name = "团购开始时间")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual DateTime StartTime { get; set; }

        [Display(Name = "团购结束时间")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual DateTime ExpireTime { get; set; }

        [Display(Name = "团购有效天数")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int ValidDay { get; set; }

    }
}
