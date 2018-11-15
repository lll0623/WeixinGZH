using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.SlideImages;



namespace LesoftWuye2.Application.ProductSlideImages.Dto
{
    [AutoMap(typeof(ProductSlideImage))]
    public class ProductCreateSlideImageInput
    {

        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.SlideImages.SlideImage.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        public long ProductId { get; set; }
    }
}
