using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Products;
using System.Collections.Generic;

namespace LesoftWuye2.Application.Products.Dto
{
    [AutoMap(typeof (Product))]
    public class CreateProductInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "规格")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxSpecificationseLength)]
        [Required]
        public string Specification { get; set; }

        [Display(Name = "单位")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxUnitLength)]
        [Required]
        public string Unit { get; set; }

        [Display(Name = "是否销售中")]
        public bool IsSale { get; set; }

        [Display(Name = "价格")]
        public decimal Price { get; set; }

        [Display(Name = "销售价格")]
        public decimal SalePrice { get; set; }

        [Display(Name = "业主价格")]
        public decimal MemberPrice { get; set; }

        //[Display(Name = "服务标签")]
        //[MaxLength(LesoftWuye2.Core.Products.Product.MaxServiceTagsLength)]
        //[Required]
        //public string ServiceTags { get; set; }

        public List<string> Tags { get; set; }


        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "轮播图片")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxSlideImagesLength)]
        public string SlideImages { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        [Display(Name = "商品简介")]
        [MaxLength(LesoftWuye2.Core.Products.Product.MaxSummaryLength)]
        public string Summary { get; set; }

        [Display(Name = "商品详情")]
        [Required]
        public string Content { get; set; }


        public long CategoryId { get; set; }

       


        public long SupplierId { get; set; }

        public virtual int SellCount { get; set; }


    }
}
